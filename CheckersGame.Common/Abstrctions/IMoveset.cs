using CheckersGame.Common.Core;

namespace CheckersGame.Common.Abstrctions
{
    public interface IMoveset
    {
        Cell[] GetValidatedEnemyCells(Cell[] cells, BaseBoard board);
    }
}
