using CheckersGame.Common.Abstractions;
using CheckersGame.Common.Abstractions.Board;
using CheckersGame.Common.Abstractions.Checker;

namespace CheckersGame.Common.Impl.International.Checker
{
    internal class InternationalBasicMoveset : IMoveset
    {
        public (Cell? EnemyCell, bool IsLegalMove) GetInfo(Cell from, Cell to, IBoard board)
        {
            throw new NotImplementedException();
        }
    }
}