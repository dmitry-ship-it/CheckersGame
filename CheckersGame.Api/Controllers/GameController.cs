using CheckersGame.Api.Models;
using CheckersGame.Common.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace CheckersGame.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGame _game;
        private readonly ILogger<GameController> _logger;

        public GameController(IGame game, ILogger<GameController> logger)
        {
            _game = game;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Get([FromBody] MoveModel moveModel)
        {
            try
            {
                _game.NextTurn((moveModel.From.Row, moveModel.From.Col), (moveModel.To.Row, moveModel.To.Col));
                _logger.LogInformation("Game moved");
            }
            catch (ArgumentException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Ok(ex.Message);
            }

            return Ok(new
            {
                Board = _game.Board,
                Turn = _game.Turn.Color
            });
        }

        [HttpGet("board")]
        public IActionResult GetBoard()
        {
            var list = new List<List<string?>>();
            for (int i = 0; i < _game.Board.Rows; i++)
            {
                list.Add(new List<string?>());
                for (int j = 0; j < _game.Board.Cols; j++)
                {
                    list[i].Add(
                        _game.Board[i, j]?.Checker?.Color.Name
                        + _game.Board[i, j]?.Checker?.ToString());
                }
            }

            return Ok(list);
        }

        [HttpGet("example")]
        public IActionResult Example()
        {
            return Ok(new MoveModel()
            {
                From = new()
                {
                    Row = 5,
                    Col = 1
                },
                To = new()
                {
                    Row = 6,
                    Col = 2
                }
            });
        }
    }
}