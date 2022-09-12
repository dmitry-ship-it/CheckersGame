namespace CheckersGame.Api.Models
{
    public class UpdateGameModel
    {
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
    }
}
