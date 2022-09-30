using CheckersGame.Api.Core;
using CheckersGame.Api.Extensions;
using CheckersGame.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CheckersGame.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly GamesVault _games;
        private readonly ILogger<GameController> _logger;

        public GameController(GamesVault games, ILogger<GameController> logger)
        {
            _games = games;
            _logger = logger;
        }

        [HttpGet("types")]
        public IActionResult GetGameTypes()
        {
            _logger.LogInformation("Total games count: {Count}", _games.Vault.Count);
            return Ok(Enum.GetNames<GameType>());
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetGamesList(
            [FromBody] IEnumerable<GameInfoModel> clientPendingGamesState,
            CancellationToken cancellationToken = default)
        {
            // TODO: replace log text
            _logger.LogInformation("Update pending list state.");

            // put thread into long polling loop
            // if 'PendingGames' changes than loop will be cancelled
            try
            {
                await LongPolling.WaitWhileAsync(
                    condition: () => clientPendingGamesState
                        .Select(g => KeyValuePair.Create(g.GameId, g.IsPending))
                        .SequenceEqual(_games.Vault.GetAllGamesStatus()),
                    frequency: 2_000,
                    timeout: 60_000,
                    cancellationToken: cancellationToken);
            }
            catch (TimeoutException)
            {
                return NoContent();
            }

            // TODO: optimize linq query
            return Ok(_games.Vault.GetAllGamesStatus().Select(pair =>
            {
                var game = _games.Get(pair.Key);

                if (game is null)
                {
                    _games.Vault.RemoveIfExist(pair.Key);
                    return null;
                }

                // TODO: Add IsRejoinable param to this model
                // also add it to frontend
                return new GameInfoModel
                {
                    GameId = pair.Key,
                    GameType = game.GameType,
                    FirstPlayerName = game.Players.First.Name,
                    SecondPlayerName = game.Players.Second.Name,
                    IsPending = _games.Vault.IsGamePending(pair.Key),
                };
            }).Where(item => item is not null));
        }

        [HttpPost("new")]
        public IActionResult StartGame(NewGameRequestModel newGameModel)
        {
            if (!Enum.TryParse<GameType>(newGameModel.GameType, out var gameType))
            {
                return BadRequest();
            }

            var game = _games.Create(gameType, newGameModel.PlayerName);

            // set this new game instance to pending state
            _games.Vault.SetGameStatus(game.GameId, true);

            return Ok(game.CreateViewModel(game.Players.First.Id));
        }

        [HttpPost("join")]
        public IActionResult Join([FromBody] JoinModel joinModel)
        {
            var game = _games.Get(joinModel.GameId);

            if (game is null || !_games.Vault.IsGamePending(joinModel.GameId))
            {
                return BadRequest();
            }

            // validation passed, so
            // set game instance to not pending i.e. started
            _games.Vault.SetGameStatus(joinModel.GameId, false);

            // set second player name
            game.Players.Second.Name = !string.IsNullOrEmpty(joinModel.SecondPlayerName)
                ? joinModel.SecondPlayerName
                : "Noname_second";

            return Ok(game.CreateViewModel(game.Players.Second.Id, joinModel.GameId));
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
            var game = _games.Get(moveModel.GameId);

            if (game is null)
            {
                if (_games.Vault.RemoveIfExist(moveModel.GameId))
                {
                    _logger.LogError("WTF is this?? Game was not deleted from key storage.");
                }

                return BadRequest();
            }

            if (_games.Vault.IsGamePending(game.GameId))
            {
                return BadRequest();
            }

            if (game.IsEnded)
            {
                _games.Vault.RemoveIfExist(game.GameId);
                return Ok("Game ended!");
            }

            if (game.CurrentPlayerTurn.Id != moveModel.PlayerId)
            {
                return BadRequest();
            }

            try
            {
                game.NextTurn(moveModel.From, moveModel.To);
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
            {
                return BadRequest(ex.Message);
            }

            // TODO: fix this log (throws NullReferenceException)
            //_logger.LogInformation("Checker moved by '{PlayerColor}' player in game (id = {GameId}).",
            //   gameContainer.Board[moveModel.From]!.Color.Name, moveModel.GameId);

            return Ok();
        }

        [HttpPost("updated")]
        public async Task<IActionResult> GetUpdatedGame(
            GameModel clientGameState,
            CancellationToken cancellationToken = default)
        {
            var game = _games.Get(clientGameState.Id);

            if (game is null)
            {
                if (_games.Vault.RemoveIfExist(clientGameState.Id))
                {
                    _logger.LogError("WTF is this?? Game was not deleted from key storage.");
                }

                return BadRequest();
            }

            try
            {
                // put into long polling loop and wait while game board is not updated
                // to update game board other player should move his checker
                await LongPolling.WaitWhileAsync(
                    condition: () => clientGameState == game,
                    timeout: 60_000, // 1 min
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex) when (ex is TimeoutException or TaskCanceledException)
            {
                return NoContent();
            }

            _logger.LogInformation("Get updated game (id = {GameId}).",
                clientGameState.Id);

            return Ok(game.CreateViewModel(
                clientGameState.PlayerId,
                clientGameState.Id));
        }
    }
}
