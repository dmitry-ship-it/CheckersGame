using CheckersGame.Api.Core;
using CheckersGame.Api.Models;
using CheckersGame.Common;
using CheckersGame.Common.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CheckersGame.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private static readonly Dictionary<Guid, bool> _games = new();

        private readonly IMemoryCache _memoryCache;
        private readonly GameFactory _gameFactory;
        private readonly ILogger<GameController> _logger;

        public GameController(IMemoryCache memoryCache, GameFactory gameFactory, ILogger<GameController> logger)
        {
            _memoryCache = memoryCache;
            _gameFactory = gameFactory;
            _logger = logger;
        }

        [HttpGet("types")]
        public IActionResult GetGameTypes()
        {
            return Ok(Enum.GetNames<GameType>());
        }

        [HttpGet("pending")]
        public IActionResult GetPendingGames()
        {
            return Ok(_games.Select(pair =>
            {
                var game = _memoryCache.Get<IGame>(pair.Key);

                return new
                {
                    Id = pair.Key,
                    Name = nameof(game) // FIXME: returns 'game' - invalid
                                        // but should be 'International' or equivalent
                };
            }));
        }

        [HttpGet("start-new")]
        public IActionResult StartGame(string gameType)
        {
            GameType game;
            try
            {
                game = Enum.Parse<GameType>(gameType);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }

            var gameId = Guid.NewGuid();
            var gameInstance = _gameFactory.CreateNew(game);
            _memoryCache.Set(
                key: gameId,
                value: gameInstance,
                TimeSpan.FromMinutes(20));

            // game pending
            _games[gameId] = true;

            return Ok(new UpdateModel
            {
                Id = gameId,
                Board = gameInstance.Board,
                CurrentPlayerTurn = gameInstance.CurrentPlayerTurn.Color.Name,
                IsEnded = false
            });
        }

        [HttpPost("join")]
        public IActionResult Join([FromBody] Guid gameId)
        {
            if (!_games.ContainsKey(gameId))
            {
                return BadRequest();
            }

            if (!_games[gameId])
            {
                return BadRequest("Game already started.");
            }

            // game started
            _games[gameId] = false;
            var game = _memoryCache.Get<IGame>(gameId);

            return Ok(new UpdateModel
            {
                Id = gameId,
                Board = game.Board,
                CurrentPlayerTurn = game.CurrentPlayerTurn.Color.Name,
                IsEnded = game.EndMessage is not null
            });
        }

        [HttpPost("update")]
        public IActionResult UpdateGameState([FromBody] MoveModel moveModel)
        {
            IGame game;

            try
            {
                game = (IGame)_memoryCache.Get(moveModel.GameId);
                game.NextTurn(moveModel.From, moveModel.To);
                _logger.LogInformation("Game moved.");
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
            {
                _logger.LogInformation("Game not moved.");
                return BadRequest(ex.Message);
            }

            return Ok(new UpdateModel
            {
                Id = moveModel.GameId,
                Board = game.Board,
                CurrentPlayerTurn = game.CurrentPlayerTurn.Color.Name,
                IsEnded = game.EndMessage is not null
            });
        }
    }
}