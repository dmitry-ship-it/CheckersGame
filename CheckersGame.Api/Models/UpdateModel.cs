namespace CheckersGame.Api.Models
{
    public class UpdateModel
    {
        public Guid Id { get; set; }

        public Guid PlayerId { get; set; }

        public IEnumerable<string?> Board { get; set; } = null!;

        public string CurrentPlayerTurn { get; set; } = null!;

        public bool IsEnded { get; set; }
    }
}
