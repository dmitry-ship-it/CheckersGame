using CheckersGame.Api.Core;

namespace CheckersGame.Api.Models
{
    public class NewGameModel
    {
        public GameType GameType { get; set; }

        public string? PlayerName { get; set; }
    }
}
