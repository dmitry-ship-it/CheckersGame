namespace CheckersGame.Api.Models
{
    public class NewGameRequestModel
    {
        public string GameType { get; set; } = default!;

        public string PlayerName { get; set; } = default!;
    }
}
