using CheckersGame.Common.Abstractions;
using CheckersGame.Common.Abstractions.Checker;
using CheckersGame.Common.Impl.General;
using CheckersGame.Common.Impl.International.Checker;
using System.Drawing;

namespace CheckersGame.Common.Impl.International.Board
{
    internal class InternationalBoard : BaseBoard
    {
        public InternationalBoard() : base(GenerateBoard())
        {
        }

        private static IChecker[,] GenerateBoard()
        {
            var board = new InternationalBasicChecker[10, 10];

            FillRows(board, 0, 4, new InternationalBasicChecker(Color.White));
            FillRows(board, 4, 2, InternationalBasicChecker.Empty);
            FillRows(board, 6, 4, new InternationalBasicChecker(Color.Black));

            return board;
        }

        private static void FillRows(IChecker[,] board, int start, int count, IChecker checker)
        {
            for (var i = start; i < start + count; i++)
            {
                for (var j = 0; j < board.GetLength(1); j++)
                {
                    if ((i + j) % 2 != 0)
                    {
                        board[i, j] = (IChecker)checker.Clone();
                    }
                }
            }
        }

        public override void HandleMove(Cell from, Cell to, out bool isCheckerBeaten)
        {
            if (this[from] == InternationalBasicChecker.Empty)
            {
                throw new ArgumentException("This cell is empty", nameof(from));
            }

            var (enemyCell, isLegalMove) = this[from].Moveset.GetInfo(from, to, this);

            if (!isLegalMove)
            {
                throw new InvalidOperationException("This move is illegal");
            }

            if (enemyCell is not null)
            {
                this[enemyCell.Value] = InternationalBasicChecker.Empty;
            }

            isCheckerBeaten = enemyCell.HasValue;

            NewMethod(from, to);
            SwapCheckers(from, to);
        }

        private void NewMethod(Cell from, Cell to)
        {
            if (this[from].Color == Color.White && to.Row == Rows - 1)
            {
                this[from] = new InternationalStrongChecker(Color.White);
            }
            else if (this[from].Color == Color.Black && to.Row == 0)
            {
                this[from] = new InternationalStrongChecker(Color.Black);
            }
        }

        private void SwapCheckers(Cell from, Cell to)
        {
            (this[to], this[from]) = (this[from], this[to]);
        }
    }
}