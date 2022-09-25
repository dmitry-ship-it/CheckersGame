namespace CheckersGame.Api.Models
{
    public class GameInfoModel : BaseModel
    {
        public string GameType { get; set; } = default!;

        public string? FirstPlayerName { get; set; }

        public string? SecondPlayerName { get; set; }

        public bool IsPending { get; set; }
    }
}
