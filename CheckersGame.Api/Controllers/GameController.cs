using CheckersGame.Api.Core;
using CheckersGame.Api.Extensions;
using CheckersGame.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel;

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
        } // TODO: Replace all models creation with AutoMapper

        [HttpGet("types")]
        public IActionResult GetGameTypes()
        {
            _logger.LogInformation("Total games count: {Count}", GameContainer.GetGamesCount());
            return Ok(Enum.GetNames<GameType>());
        }

        [HttpPost("pending")]
        public async Task<IActionResult> GetPendingGames(
            [FromBody] IEnumerable<PendingGameModel> clientPendingGamesState,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Update pending list state.");

            // put thread into long polling loop
            // if 'PendingGames' changes than loop will be cancelled
            try
            {
                await LongPolling.WaitWhileAsync(
                    condition: () => clientPendingGamesState
                        .Select(g => g.GameId)
                        .SequenceEqual(GetPendingGameIds()),
                    frequency: 2_000,
                    timeout: 60_000,
                    cancellationToken: cancellationToken);
            }
            catch (TimeoutException)
            {
                return NoContent();
            }

            return Ok(GetPendingGameIds().Select(gameId =>
            {
                var gameContainer = _cache.Get<GameContainer>(gameId);

                // TODO: Add IsRejoinable param to this model
                // also add it to frontend
                return new PendingGameModel
                {
                    GameId = gameId,
                    GameType = gameContainer.GameType,
                    FirstPlayerName = gameContainer.Players.First.Name
                };
            }));
        }

        [HttpPost("new")]
        public IActionResult StartGame(NewGameModel newGameModel)
        {
            if (!Enum.TryParse<GameType>(newGameModel.GameType, out var gameType))
            {
                return BadRequest();
            }

            var container = _gameFactory.Create(gameType, newGameModel.PlayerName);

            _cache.Set(
                key: container.GameId,
                value: container,
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)));

            // set this new game instance to pending state
            GameContainer.SetGameStatus(container.GameId, true);

            // TODO: move object creation
            return Ok(new UpdateModel
            {
                Id = container.GameId,
                PlayerId = container.Players.First.Id,
                FirstPlayerName = container.Players.First.Name,
                SecondPlayerName = container.Players.Second.Name,
                Board = container.Board,
                CurrentPlayerTurn = container.CurrentPlayerTurn.Id.ToString(),
                IsEnded = container.IsEnded
            });
        }

        [HttpPost("join")]
        public IActionResult Join([FromBody] JoinModel joinModel)
        {
            if (!GameContainer.IsGameExists(joinModel.GameId)
                || !GameContainer.GetGameStatus(joinModel.GameId))
            {
                return BadRequest();
            }

            // set game instance to not pending i.e. started
            GameContainer.SetGameStatus(joinModel.GameId, false);

            // get game instance and set second player
            var gameContainer = _cache.Get<GameContainer>(joinModel.GameId);
            gameContainer.Players.Second.Name = joinModel.SecondPlayerName!;

            // TODO: move game creation 
            return Ok(new UpdateModel
            {
                Id = joinModel.GameId,
                PlayerId = gameContainer.Players.Second.Id,
                FirstPlayerName = gameContainer.Players.First.Name,
                SecondPlayerName = gameContainer.Players.Second.Name,
                Board = gameContainer.Board,
                CurrentPlayerTurn = gameContainer.CurrentPlayerTurn.Id.ToString(),
                IsEnded = gameContainer.IsEnded
            });
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

            _logger.LogInformation("Checker moved by player (id = {PlayerId}) in game (id = {GameId}).",
               moveModel.PlayerId, moveModel.PlayerId);

            if (gameContainer.IsEnded)
            {
                GameContainer.RemoveGame(gameContainer.GameId);
                return Ok("Game ended!");
            }

            return Ok();
        }

        [HttpPost("updated")]
        public async Task<IActionResult> GetUpdatedGame(
            GameFieldModel clientGameState,
            CancellationToken cancellationToken = default)
        {
            var gameContainer = _cache.Get<GameContainer>(clientGameState.GameId);

            try
            {
                // put into long polling loop while game board not updated
                // to update game board other player should move his checker
                await LongPolling.WaitWhileAsync(
                    condition: () => gameContainer.Board.SequenceEqual(clientGameState.Board),
                    timeout: 60_000, // 1 min
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex) when (ex is TimeoutException or TaskCanceledException)
            {
                return NoContent();
            }

            _logger.LogInformation("Get updated game (id = {GameId}) by player (id = {PlayerId}).",
                clientGameState.GameId, clientGameState.PlayerId);

            // TODO: Add player names into model
            // TODO: Replace this model creation with AutoMapper
            // may be add new model for it?
            // TODO: Add base model with GameId and ??
            // !! Make similar changes to frontend
            return Ok(new UpdateModel
            {
                Id = clientGameState.GameId,
                PlayerId = clientGameState.PlayerId,
                FirstPlayerName = gameContainer.Players.First.Name,
                SecondPlayerName = gameContainer.Players.Second.Name,
                Board = gameContainer.Board,
                CurrentPlayerTurn = gameContainer.CurrentPlayerTurn.Id.ToString(),
                IsEnded = gameContainer.IsEnded
            });
        }

        [NonAction]
        private IEnumerable<Guid> GetPendingGameIds()
        {
            return GameContainer.GetAllGamesStatus().Select(pair =>
                pair.Value && _cache.TryGetValue<GameContainer>(pair.Key, out var _)
                    ? pair.Key
                    : Guid.Empty)
                        .Where(gameId => gameId != Guid.Empty);
        }
    }
}