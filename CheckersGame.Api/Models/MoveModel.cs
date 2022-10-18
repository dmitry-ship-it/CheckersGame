using CheckersGame.Common;

namespace CheckersGame.Api.Models
{
    public class MoveModel : BaseModel
    {
        public Guid PlayerId { get; set; }

        public Cell[] Cells { get; set; } = null!;
    }
}
