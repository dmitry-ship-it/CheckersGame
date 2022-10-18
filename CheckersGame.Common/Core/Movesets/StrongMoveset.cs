using CheckersGame.Common.Abstrctions;
using CheckersGame.Common.Core;
using CheckersGame.Common.Core.Movesets;

namespace CheckersGame.Common.Impl.International.Checker
{
    internal sealed class StrongMoveset : IMoveset
    {
        private static IMoveset _instance = new StrongMoveset();
        public static IMoveset Instance => _instance ??= new StrongMoveset();

        private StrongMoveset()
        { }

        public Cell[] GetValidatedEnemyCells(Cell[] cells, BaseBoard board)
        {
            return MovesetHelper.ValidateAndGetEnemies(cells, board);
        }

        public override string ToString()
        {
            return "Strong";
        }
    }
}