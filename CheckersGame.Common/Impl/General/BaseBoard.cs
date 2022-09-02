using CheckersGame.Common.Abstractions;
using CheckersGame.Common.Abstractions.Board;
using CheckersGame.Common.Abstractions.Checker;
using System.Collections;

namespace CheckersGame.Common.Impl.General
{
    internal abstract class BaseBoard : IBoard
    {
        private readonly IChecker?[,] _board;

        protected BaseBoard(IChecker?[,] board)
        {
            _board = board;
            Rows = board.GetLength(0);
            Cols = board.GetLength(1);
        }

        public IChecker? this[Cell cell]
        {
            get => this[cell.Row, cell.Col];
            set => this[cell.Row, cell.Col] = value;
        }

        public IChecker? this[int row, int col]
        {
            get => _board[row, col];
            set => _board[row, col] = value;
        }

        public int Rows { get; init; }
        public int Cols { get; init; }

        public IEnumerator<string?> GetEnumerator()
        {
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Cols; j++)
                {
                    yield return _board[i, j]?.ToString();
                }
            }
        }

        public abstract void HandleMove(Cell from, Cell to, out bool isCheckerBeaten);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}