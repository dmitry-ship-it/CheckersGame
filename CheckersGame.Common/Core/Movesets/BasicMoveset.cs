using CheckersGame.Common.Abstrctions;
using CheckersGame.Common.Core;
using CheckersGame.Common.Core.Movesets;

namespace CheckersGame.Common.Impl.International.Checker
{
    internal sealed class BasicMoveset : IMoveset
    {
        private static IMoveset _instance = new BasicMoveset();
        public static IMoveset Instance => _instance ??= new BasicMoveset();

        private BasicMoveset()
        { }

        public Cell[] GetValidatedEnemyCells(Cell[] cells, BaseBoard board)
        {
            return MovesetHelper.ValidateAndGetEnemies(cells, board);
        }

        public override string ToString()
        {
            return "Basic";
        }
    }
}