using CheckersGame.Common.Abstractions;
using CheckersGame.Common.Abstractions.Board;
using CheckersGame.Common.Abstractions.Checker;

namespace CheckersGame.Common.Impl.International.Checker
{
    internal sealed class InternationalBasicMoveset : IMoveset
    {
        private static IMoveset _instance = new InternationalBasicMoveset();
        public static IMoveset Instance => _instance ??= new InternationalBasicMoveset();

        private InternationalBasicMoveset() { }

        public (Cell? EnemyCell, bool IsLegalMove) GetInfo(Cell from, Cell to, IBoard board)
        {
            throw new NotImplementedException();
        }
    }
}