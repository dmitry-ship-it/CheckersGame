using CheckersGame.Common.Abstractions;
using CheckersGame.Common.Abstractions.Board;
using CheckersGame.Common.Implementations.International.Board;
using CheckersGame.Common.Implementations.International.Checker;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame.Common.Implementations.International
{
    public class Game : IGame
    {
        public IBoard Board { get; init; }
        public IPlayer BlackPlayer { get; init; }
        public IPlayer WhitePlayer { get; init; }

        public IPlayer Turn => _isWhiteTurn ? WhitePlayer : BlackPlayer;

        private bool _isWhiteTurn = true;

        public Game()
        {
            Board = new Board.Board();
            BlackPlayer = new Player(Board, Color.Black);
            WhitePlayer = new Player(Board, Color.White);
        }

        public void NextTurn(
            (int row, int col) from,
            (int row, int col) to)
        {
            if ((Board[from.row, from.col]?.Checker?.Color == WhitePlayer.Color && !_isWhiteTurn)
                || (Board[from.row, from.col]?.Checker?.Color == BlackPlayer.Color && _isWhiteTurn))
            {
                throw new ArgumentException("This player can't touch this checker.");
            }

            Board.MoveChecker(from, to, _isWhiteTurn ? Color.White : Color.Black);
            var winner = GetWinner();
            if (winner is not null) // Make this as message
            {
                throw new InvalidOperationException($"Winner is {winner.Color}.");
            }

            // when swap is successful
            if (Board[to.row, to.col]?.Checker?.Color == WhitePlayer.Color
                && to.row == Board.Rows - 1)
            {
                Board[to.row, to.col] = new Cell(new StrongChecker(WhitePlayer.Color));
            }
            if (Board[to.row, to.col]?.Checker?.Color == BlackPlayer.Color
                && to.row == 0)
            {
                Board[to.row, to.col] = new Cell(new StrongChecker(BlackPlayer.Color));
            }

            _isWhiteTurn = !_isWhiteTurn;
        }

        public IPlayer? GetWinner()
        {
            if (GetCheckers(WhitePlayer.Color) == 0)
            {
                return BlackPlayer;
            }
            else if (GetCheckers(BlackPlayer.Color) == 0)
            {
                return WhitePlayer;
            }
            else
            {
                return null;
            }
        }

        private int GetCheckers(Color color)
        {
            var count = 0;

            for (var i = 0; i < Board.Rows; i++)
            {
                for (var j = 0; j < Board.Cols; j++)
                {
                    if (Board[i, j]?.Checker?.Color == color)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
