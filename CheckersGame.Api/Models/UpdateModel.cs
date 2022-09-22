using CheckersGame.Api.Core;

namespace CheckersGame.Api.Models
{
    public class UpdateModel
    {
        public Guid Id { get; set; }

        public Guid PlayerId { get; set; }

        public string FirstPlayerName { get; set; } = default!;
        public string SecondPlayerName { get; set; } = default!;

        public IEnumerable<string?> Board { get; set; } = default!;

        public string CurrentPlayerTurn { get; set; } = default!;

        public bool IsEnded { get; set; }
    }
}
