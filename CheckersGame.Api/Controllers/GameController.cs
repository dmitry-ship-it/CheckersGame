using CheckersGame.Api.Core;
using CheckersGame.Api.Extensions;
using CheckersGame.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CheckersGame.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly GameContainerFactory _gameFactory;
        private readonly ILogger<GameController> _logger;

        public GameController(
            IMemoryCache memoryCache,
            GameContainerFactory gameFactory,
            ILogger<GameController> logger)
        {
            _cache = memoryCache;
            _gameFactory = gameFactory;
            _logger = logger;
        }

        [HttpGet("types")]
        public IActionResult GetGameTypes()
        {
            _logger.LogInformation("Total games count: {Count}", GameContainer.GetGamesCount());
            return Ok(Enum.GetNames<GameType>());
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetPendingGames(
            [FromBody] IEnumerable<GameInfoModel> clientPendingGamesState,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Update pending list state.");

            // put thread into long polling loop
            // if 'PendingGames' changes than loop will be cancelled
            try
            {
                await LongPolling.WaitWhileAsync(
                    condition: () => clientPendingGamesState
                        .Select(g => KeyValuePair.Create(g.GameId, g.IsPending))
                        .SequenceEqual(GameContainer.GetAllGamesStatus()),
                    frequency: 2_000,
                    timeout: 60_000,
                    cancellationToken: cancellationToken);
            }
            catch (TimeoutException)
            {
                return NoContent();
            }

            return Ok(GameContainer.GetAllGamesStatus().Select(pair =>
            {
                var gameContainer = _cache.Get<GameContainer>(pair.Key);

                // TODO: Add IsRejoinable param to this model
                // also add it to frontend
                return new GameInfoModel
                {
                    GameId = pair.Key,
                    GameType = gameContainer.GameType,
                    FirstPlayerName = gameContainer.Players.First.Name,
                    SecondPlayerName = gameContainer.Players.Second.Name,
                    IsPending = GameContainer.IsGamePending(pair.Key),
                };
            }));
        }

        [HttpPost("new")]
        public IActionResult StartGame(NewGameRequestModel newGameModel)
        {
            if (!Enum.TryParse<GameType>(newGameModel.GameType, out var gameType))
            {
                return BadRequest();
            }

            var container = _gameFactory.Create(gameType, newGameModel.PlayerName);

            var cacheOptions = new MemoryCacheEntryOptions();
            cacheOptions.SetSlidingExpiration(TimeSpan.FromMinutes(5)); // remove game from dictionary on expiration
            cacheOptions.RegisterPostEvictionCallback((key, _, _, _) => GameContainer.RemoveGame((Guid)key));
            _cache.Set(container.GameId, container, cacheOptions);

            // set this new game instance to pending state
            GameContainer.SetGameStatus(container.GameId, true);

            return Ok(container.CreateGameModel(container.Players.First.Id));
        }

        [HttpPost("join")]
        public IActionResult Join([FromBody] JoinModel joinModel)
        {
            if (!GameContainer.IsGameExists(joinModel.GameId)
                || !GameContainer.IsGamePending(joinModel.GameId))
            {
                return BadRequest();
            }

            // set game instance to not pending i.e. started
            GameContainer.SetGameStatus(joinModel.GameId, false);

            // get game instance and set second player
            var container = _cache.Get<GameContainer>(joinModel.GameId);
            if (!string.IsNullOrEmpty(joinModel.SecondPlayerName))
            {
                container.Players.Second.Name = joinModel.SecondPlayerName;
            }

            return Ok(container.CreateGameModel(container.Players.Second.Id, joinModel.GameId));
        }

        [HttpPost("rejoin")]
        public IActionResult Rejoin(/* REPLACE THIS [FromBody] JoinModel joinModel */)
        {
            // var gameContainer = _cache.Get<GameContainer>(joinModel.GameId);

            // TODO: implement server side rejoin feature
            // replace all PlayerIds with session 
            throw new NotImplementedException();
        }

        [HttpPost("update")]
        public IActionResult UpdateGameState([FromBody] MoveModel moveModel)
        {
            var gameContainer = _cache.Get<GameContainer>(moveModel.GameId);

            if (gameContainer is null)
            {
                if (GameContainer.IsGameExists(moveModel.GameId))
                {
                    GameContainer.RemoveGame(moveModel.GameId);
                }

                return BadRequest();
            }

            if (gameContainer.IsEnded)
            {
                GameContainer.RemoveGame(gameContainer.GameId);
                return Ok("Game ended!");
            }

            if (gameContainer.CurrentPlayerTurn.Id != moveModel.PlayerId)
            {
                return BadRequest();
            }

            try
            {
                gameContainer.NextTurn(moveModel.From, moveModel.To);
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
            {
                return BadRequest(ex.Message);
            }

            //_logger.LogInformation("Checker moved by '{PlayerColor}' player in game (id = {GameId}).",
            //   gameContainer.Board[moveModel.From]!.Color.Name, moveModel.GameId);

            return Ok();
        }

        [HttpPost("updated")]
        public async Task<IActionResult> GetUpdatedGame(
            GameModel clientGameState,
            CancellationToken cancellationToken = default)
        {
            var container = _cache.Get<GameContainer>(clientGameState.Id);

            if (container is null)
            {
                if (GameContainer.IsGameExists(clientGameState.Id))
                {
                    GameContainer.RemoveGame(clientGameState.Id);
                }

                return BadRequest();
            }

            try
            {
                // put into long polling loop and wait while game board is not updated
                // to update game board other player should move his checker
                await LongPolling.WaitWhileAsync(
                    condition: () => clientGameState == container,
                    timeout: 60_000, // 1 min
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex) when (ex is TimeoutException or TaskCanceledException)
            {
                return NoContent();
            }

            _logger.LogInformation("Get updated game (id = {GameId}).",
                clientGameState.Id);

            return Ok(container.CreateGameModel(
                clientGameState.PlayerId,
                clientGameState.Id));
        }
    }
}
