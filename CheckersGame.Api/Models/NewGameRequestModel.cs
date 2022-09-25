namespace CheckersGame.Api.Models
{
    public class NewGameRequestModel
    {
        public string GameType { get; set; } = null!;

        public string? PlayerName { get; set; }
    }
}
