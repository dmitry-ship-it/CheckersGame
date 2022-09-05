namespace CheckersGame.Api.Models
{
    public class NewGameModel
    {
        public string GameType { get; set; } = null!;

        public string? PlayerName { get; set; }
    }
}
