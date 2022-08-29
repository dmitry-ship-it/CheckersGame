using CheckersGame.Common.Abstractions.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame.Common.Abstractions
{
    public interface IGame
    {
        IBoard Board { get; init; }

        IPlayer BlackPlayer { get; init; }

        IPlayer WhitePlayer { get; init; }

        IPlayer Turn { get; }
        void NextTurn(
                    (int row, int col) from,
                    (int row, int col) to);
    }
}
