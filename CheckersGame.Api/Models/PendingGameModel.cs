namespace CheckersGame.Api.Models
{
    public class PendingGameModel
    {
        public Guid GameId { get; set; }

        public string TypeName { get; set; } = default!;
    }
}
