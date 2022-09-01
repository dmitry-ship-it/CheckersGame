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
        private static readonly Dictionary<Guid, bool> _gamesStarted = new();

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
            return Ok(Enum.GetNames<GameType>());
        }

        [HttpGet("pending")]
        public IActionResult GetPendingGames()
        {
            return Ok(_gamesStarted.Select(pair =>
            {
                var gameContainer = _cache.Get<GameContainer>(pair.Key);

                return new PendingGameModel
                {
                    GameId = pair.Key,
                    TypeName = gameContainer.Game
                        .GetType().Name
                        .Replace("Game", string.Empty)
                };
            }));
        }

        [HttpPost("new")]
        public IActionResult StartGame(NewGameModel newGameModel)
        {
            var container = _gameFactory.Create(
                newGameModel.GameType,
                newGameModel.PlayerName);

            _cache.Set(
                key: container.GameId,
                value: container,
                TimeSpan.FromMinutes(20));

            // set game pending
            _gamesStarted[container.GameId] = true;

            return Ok(new UpdateModel
            {
                Id = container.GameId,
                PlayerId = container.FirstPlayer.Id,
                Board = container.Game.Board,
                CurrentPlayerTurn = container.Game.CurrentPlayerTurn.Color.Name,
                IsEnded = container.IsEnded
            });
        }

        [HttpPost("join")]
        public IActionResult Join([FromBody] JoinModel joinModel)
        {
            if (!_gamesStarted.ContainsKey(joinModel.GameId))
            {
                return BadRequest();
            }

            if (!_gamesStarted[joinModel.GameId])
            {
                return BadRequest("Game already started.");
            }

            // game started
            _gamesStarted[joinModel.GameId] = false;
            var gameContainer = _cache.Get<GameContainer>(joinModel.GameId);
            gameContainer.SecondPlayer.Name = joinModel.SecondPlayerName!;

            return Ok(new UpdateModel
            {
                Id = joinModel.GameId,
                PlayerId = gameContainer.SecondPlayer.Id,
                Board = gameContainer.Game.Board,
                CurrentPlayerTurn = gameContainer.Game.CurrentPlayerTurn.Color.Name,
                IsEnded = gameContainer.IsEnded
            });
        }

        [HttpPost("update")]
        public IActionResult UpdateGameState([FromBody] MoveModel moveModel)
        {
            GameContainer gameContainer;

            try
            {
                gameContainer = _cache.Get<GameContainer>(moveModel.GameId);
                _logger.LogInformation("In game (id = {GameId}) player.", gameContainer.GameId);
                gameContainer.Game.NextTurn(moveModel.From, moveModel.To);
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
            {
                _logger.LogInformation("Game not moved.");
                return BadRequest(ex.Message);
            }

            return Ok(new UpdateModel
            {
                Id = moveModel.GameId,
                PlayerId = moveModel.PlayerId,
                Board = gameContainer.Game.Board,
                CurrentPlayerTurn = gameContainer.Game.CurrentPlayerTurn.Color.Name,
                IsEnded = gameContainer.IsEnded
            });
        }
    }
}