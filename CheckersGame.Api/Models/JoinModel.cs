namespace CheckersGame.Api.Models
{
    public class JoinModel
    {
        public Guid GameId { get; set; }

        public string? SecondPlayerName { get; set; }
    }
}
