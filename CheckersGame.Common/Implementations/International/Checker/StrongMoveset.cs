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
    // TODO: Refactor all class. Add abstarct base type
    internal class StrongMoveset : IMoveset
    {
        // TODO: Refactor
        public bool IsLegalMove((int row, int col) from, (int row, int col) to, IBoard board)
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
            if (!IsEnemyOnTheWay(from, to, board))
            {
                return false;
            }

            // TODO: Debug this
            return true;
        }

        // TODO: Refactor (with on line 79)
        public (int row, int col)? TryGetEnemyPosOnTheWay((int row, int col) from, (int row, int col) to, IBoard board)
        {
            if (!IsEnemyOnTheWay(from, to, board))
            {
                return null; // throw new ArgumentException("There is not enemy on the way.");
            }

            var rowDirection = Math.Abs(to.row - from.row) / (to.row - from.row);
            var colDirection = Math.Abs(to.col - from.col) / (to.col - from.col);

            var allyColor = board[from.row, from.col]?.Checker?.Color;

            for (var i = from.row; i < to.row; i += rowDirection)
            {
                for (var j = from.col; j < to.row; j += colDirection)
                {
                    if (board[i, j].Checker?.Color == allyColor)
                    {
                        return null;
                    }

                    if (board[i, j].Checker is not null)
                    {
                        return (i, j);
                    }
                }
            }

            return null;
        }

        // TODO: Refactor (with on line 47)
        private bool IsEnemyOnTheWay((int row, int col) from, (int row, int col) to, IBoard board)
        {
            var rowDirection = Math.Abs(to.row - from.row) / (to.row - from.row);
            var colDirection = Math.Abs(to.col - from.col) / (to.col - from.col);

            var allyColor = board[from.row, from.col]?.Checker?.Color;

            for (int i = from.row + rowDirection, j = from.col + colDirection;
                i * rowDirection < to.row && j * colDirection < to.col;
                i += rowDirection, j += colDirection)
            {
                if (board[i, j]?.Checker is not null
                    && board[i, j].Checker?.Color != allyColor)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
