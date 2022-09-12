namespace CheckersGame.Api.Models
{
    public class GameFieldModel
    {
        public Guid GameId { get; set; }

        public Guid PlayerId { get; set; }

        public IEnumerable<string?> Board { get; set; } = default!;
    }
}
