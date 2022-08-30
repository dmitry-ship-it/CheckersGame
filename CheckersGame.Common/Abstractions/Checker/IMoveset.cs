using CheckersGame.Common.Abstractions.Board;

namespace CheckersGame.Common.Abstractions.Checker
{
    public interface IMoveset
    {
        (Cell? EnemyCell, bool IsLegalMove) GetInfo(Cell from, Cell to, IBoard board);
    }
}