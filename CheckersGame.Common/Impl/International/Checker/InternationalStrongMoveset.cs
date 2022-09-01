using CheckersGame.Common.Abstractions;
using CheckersGame.Common.Abstractions.Board;
using CheckersGame.Common.Abstractions.Checker;
using CheckersGame.Common.Impl.General;

namespace CheckersGame.Common.Impl.International.Checker
{
    internal sealed class InternationalStrongMoveset : IMoveset
    {
        private static IMoveset _instance = new InternationalStrongMoveset();
        public static IMoveset Instance => _instance ??= new InternationalStrongMoveset();

        private InternationalStrongMoveset()
        { }

        public (Cell? EnemyCell, bool IsLegalMove) GetInfo(Cell from, Cell to, IBoard board)
        {
            var invalidResult = ((Cell?)null, false);

            // initial pos is empty
            // or target pos is not empty
            if (board[from] == BaseChecker.Empty
                || board[to] != BaseChecker.Empty)
            {
                return invalidResult;
            }

            var rowDiff = Math.Abs(to.Row - from.Row);
            var colDiff = Math.Abs(to.Col - from.Col);

            // if not diagonal
            if (rowDiff != colDiff)
            {
                return invalidResult;
            }

            // enemy on the way
            Cell? enemy;
            try
            {
                enemy = GetEnemyPosOnTheWay(from, to, board);
            }
            catch (ArgumentException)
            {
                return invalidResult;
            }

            if (enemy is not null)
            {
                return (enemy, true);
            }

            // no enemy but any amount of cells
            return (null, true);
        }

        private static Cell? GetEnemyPosOnTheWay(Cell from, Cell to, IBoard board)
        {
            var rowDirection = Math.Abs(to.Row - from.Row) / (to.Row - from.Row);
            var colDirection = Math.Abs(to.Col - from.Col) / (to.Col - from.Col);

            var allies = 0;
            var enemies = 0;

            Cell? pos = null;

            for (var i = from.Row + rowDirection; i < to.Row; i += rowDirection)
            {
                for (var j = from.Col + colDirection; j < to.Col; j += colDirection)
                {
                    if (board[i, j] == BaseChecker.Empty)
                    {
                        continue;
                    }

                    if (board[from]!.Color == board[i, j]?.Color)
                    {
                        allies++;
                    }
                    else
                    {
                        enemies++;
                        pos ??= new Cell { Row = i, Col = j };
                    }
                }
            }

            if (allies > 0 || enemies > 1)
            {
                throw new ArgumentException("This way is invalid");
            }

            return allies == 0 && enemies == 1
                ? pos
                : null;
        }

        public override string ToString()
        {
            return "Strong";
        }
    }
}