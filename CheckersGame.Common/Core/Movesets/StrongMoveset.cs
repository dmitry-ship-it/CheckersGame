using CheckersGame.Common.Abstrctions;
using CheckersGame.Common.Core;

namespace CheckersGame.Common.Impl.International.Checker
{
    internal sealed class StrongMoveset : IMoveset
    {
        private static IMoveset _instance = new StrongMoveset();
        public static IMoveset Instance => _instance ??= new StrongMoveset();

        private StrongMoveset()
        { }

        public (Cell? EnemyCell, bool IsLegalMove) GetInfo(Cell from, Cell to, BaseBoard board)
        {
            var invalidResult = ((Cell?)null, false);

            // initial pos is empty
            // or target pos is not empty
            if (board[from] == BaseChecker.Empty
                || board[to] != BaseChecker.Empty)
            {
                return invalidResult;
            }

            // if basic checker can't move both forward and backward;
            // check if player trying to move checker backward 
            if (!board.GameRules.IsStrongCheckerCanMoveBothWays
                &&
                ((board[from]!.DefaultMoveDirection == MoveDirection.FromTopToBottom
                && to.Row - from.Row < 0)
                ||
                (board[from]!.DefaultMoveDirection == MoveDirection.FromBottomToTop
                && to.Row - from.Row > 0)))
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

        private static Cell? GetEnemyPosOnTheWay(Cell from, Cell to, BaseBoard board)
        {
            var rowDirection = Math.Abs(to.Row - from.Row) / (to.Row - from.Row);
            var colDirection = Math.Abs(to.Col - from.Col) / (to.Col - from.Col);

            var allies = 0;
            var enemies = 0;

            Cell? pos = null;

            for (int i = from.Row + rowDirection, j = from.Col + colDirection;
                i != to.Row && j != to.Col;
                i += rowDirection, j += colDirection)
            {
                if (board[i, j] == BaseChecker.Empty)
                {
                    continue;
                }

                if (board[from]!.Color == board[i, j]!.Color)
                {
                    allies++;
                }
                else
                {
                    enemies++;
                    pos ??= new Cell { Row = i, Col = j };
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