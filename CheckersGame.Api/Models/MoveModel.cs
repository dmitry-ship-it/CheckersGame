using CheckersGame.Common.Abstractions;

namespace CheckersGame.Api.Models
{
    public class MoveModel
    {
        public Guid GameId { get; set; }

        public Cell From { get; set; }
        public Cell To { get; set; }
    }
}
