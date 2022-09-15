using CheckersGame.Common;

namespace CheckersGame.Api.Models
{
    public class MoveModel
    {
        public Guid GameId { get; set; }

        public Guid PlayerId { get; set; }

        public Cell From { get; set; }
        public Cell To { get; set; }
    }
}
