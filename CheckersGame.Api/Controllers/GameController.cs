using CheckersGame.Api.Core;
using CheckersGame.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CheckersGame.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private static readonly SortedDictionary<Guid, bool> _gamesPending = new();

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
            _logger.LogInformation("Total games count: {Count}", _gamesPending.Count);
            return Ok(Enum.GetNames<GameType>());
        }

        [HttpPost("pending")]
        public async Task<IActionResult> GetPendingGames(
            [FromBody] IEnumerable<PendingGameModel> clientGamesState,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Pending games.");

            var availableGames = _gamesPending.Select(pair =>
            {
                if (!pair.Value)
                {
                    return null;
                }

                if (!_cache.TryGetValue<GameContainer>(pair.Key, out var gameContainer))
                {
                    _gamesPending.Remove(pair.Key);
                    return null;
                }

                return (Guid?)pair.Key;
            }).Where(val => val.HasValue);

            if (clientGamesState.Select(g => (Guid?)g.GameId).SequenceEqual(availableGames))
            {
                var oldGamesCount = _gamesPending.Count;
                try
                {
                    await Extensions.LongPolling.WaitWhileAsync(
                        condition: () => _gamesPending.Count == oldGamesCount,
                        frequency: 1_000,
                        timeout: 60_000,
                        cancellationToken: cancellationToken);
                }
                catch (TimeoutException)
                {
                    return NoContent();
                }
            }

            return Ok(_gamesPending.Select(pair =>
            {
                if (!pair.Value)
                {
                    return null;
                }

                if (!_cache.TryGetValue<GameContainer>(pair.Key, out var gameContainer))
                {
                    _gamesPending.Remove(pair.Key);
                    return null;
                }

                return new PendingGameModel
                {
                    GameId = pair.Key,
                    GameType = gameContainer.GameType,
                    FirstPlayerName = gameContainer.FirstPlayer.Name
                };
            }).Where(val => val != null));
        }

        [HttpPost("new")]
        public IActionResult StartGame(NewGameModel newGameModel)
        {
            if (!Enum.TryParse<GameType>(newGameModel.GameType, out var gameType))
            {
                return BadRequest();
            }

            var container = _gameFactory.Create(
                gameType,
                newGameModel.PlayerName);

            _cache.Set(
                key: container.GameId,
                value: container,
                TimeSpan.FromMinutes(20));

            // set game pending
            _gamesPending[container.GameId] = true;

            // TODO: move object creation
            return Ok(new UpdateModel
            {
                Id = container.GameId,
                PlayerId = container.FirstPlayer.Id,
                Board = container.Board,
                CurrentPlayerTurn = container.CurrentPlayerTurn.Id.ToString(),
                IsEnded = container.IsEnded
            });
        }

        [HttpPost("join")]
        public IActionResult Join([FromBody] JoinModel joinModel)
        {
            if (!_gamesPending.ContainsKey(joinModel.GameId)
                || !_gamesPending[joinModel.GameId])
            {
                return BadRequest();
            }

            // game started
            _gamesPending[joinModel.GameId] = false;
            var gameContainer = _cache.Get<GameContainer>(joinModel.GameId);
            gameContainer.SecondPlayer.Name = joinModel.SecondPlayerName!;

            // TODO: move game creation 
            return Ok(new UpdateModel
            {
                Id = joinModel.GameId,
                PlayerId = gameContainer.SecondPlayer.Id,
                Board = gameContainer.Board,
                CurrentPlayerTurn = gameContainer.CurrentPlayerTurn.Id.ToString(),
                IsEnded = gameContainer.IsEnded
            });
        }

        [HttpPost("update")]
        public IActionResult UpdateGameState([FromBody] MoveModel moveModel)
        {
            var gameContainer = _cache.Get<GameContainer>(moveModel.GameId);

            if (gameContainer is null || gameContainer.CurrentPlayerTurn.Id != moveModel.PlayerId)
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

            // TODO: move object creation
            return Ok(new UpdateModel
            {
                Id = moveModel.GameId,
                PlayerId = moveModel.PlayerId,
                Board = gameContainer.Board,
                CurrentPlayerTurn = gameContainer.CurrentPlayerTurn.Id.ToString(),
                IsEnded = gameContainer.IsEnded
            });
        }

        [HttpPost("updated")]
        public async Task<IActionResult> GetUpdatedGame(
            GameFieldModel clientGameState,
            CancellationToken cancellationToken = default)
        {
            var gameContainer = _cache.Get<GameContainer>(clientGameState.GameId);

            if (gameContainer is null)
            {
                return BadRequest();
            }

            try
            {
                await Extensions.LongPolling.WaitWhileAsync(
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

            return Ok(new UpdateModel
            {
                Id = clientGameState.GameId,
                PlayerId = clientGameState.PlayerId,
                Board = gameContainer.Board,
                CurrentPlayerTurn = gameContainer.CurrentPlayerTurn.Id.ToString(),
                IsEnded = gameContainer.IsEnded
            });
        }
    }
}