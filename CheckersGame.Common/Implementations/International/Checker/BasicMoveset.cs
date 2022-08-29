using CheckersGame.Common.Abstractions.Board;
using CheckersGame.Common.Abstractions.Checker;
using CheckersGame.Common.Implementations.International.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame.Common.Implementations.International.Checker
{
    // TODO: Refactor all class. Create base abstract class
    internal class BasicMoveset : IMoveset
    {
        public (int row, int col)? TryGetEnemyPosOnTheWay((int row, int col) from, (int row, int col) to, IBoard board)
        {
            if (!IsEnemyOnTheWay(from, to, board))
            {
                return null; // throw new ArgumentException("There is not enemy on the way.");
            }

            var rowDirection = Math.Abs(to.row - from.row) / (to.row - from.row);
            var colDirection = Math.Abs(to.col - from.col) / (to.col - from.col);

            return (from.row + rowDirection, from.col + colDirection);
        }

        public bool IsLegalMove(
            (int row, int col) from,
            (int row, int col) to,
            IBoard board)
        {
            // inital pos is invalid
            if (board[from.row, from.col] == Cell.Empty)
            {
                return false;
            }

            // target pos is not empty
            if (board[to.row, to.col] != Cell.Empty)
            {
                return false;
            }

            var rowDiff = Math.Abs(to.row - from.row);
            var colDiff = Math.Abs(to.col - from.col);

            // not diagonal
            if (rowDiff != colDiff)
            {
                return false;
            }

            // enemy on the way
            if (rowDiff == 2 && IsEnemyOnTheWay(from, to, board))
            {
                return true;
            }

            // 1 cell by diagonal
            return rowDiff == 1
                && colDiff == 1;
        }

        private bool IsEnemyOnTheWay((int row, int col) from, (int row, int col) to, IBoard board)
        {
            var rowDirection = Math.Abs(to.row - from.row) / (to.row - from.row);
            var colDirection = Math.Abs(to.col - from.col) / (to.col - from.col);

            return board[from.row + rowDirection, from.col + colDirection]?.Checker is not null;
        }
    }
}
