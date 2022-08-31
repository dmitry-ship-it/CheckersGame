using CheckersGame.Common.Abstractions;
using CheckersGame.Common.Abstractions.Board;
using CheckersGame.Common.Abstractions.Checker;

namespace CheckersGame.Common.Impl.International.Checker
{
    internal sealed class InternationalStrongMoveset : IMoveset
    {
        private static IMoveset _instance = new InternationalStrongMoveset();
        public static IMoveset Instance => _instance ??= new InternationalStrongMoveset();

        private InternationalStrongMoveset() { }

        public (Cell? EnemyCell, bool IsLegalMove) GetInfo(Cell from, Cell to, IBoard board)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Strong";
        }
    }
}