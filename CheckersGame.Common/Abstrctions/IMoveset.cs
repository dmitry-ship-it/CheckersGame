using CheckersGame.Common.Core;

namespace CheckersGame.Common.Abstrctions
{
    public interface IMoveset
    {
        (Cell? EnemyCell, bool IsLegalMove) GetInfo(Cell from, Cell to, BaseBoard board);
    }
}
