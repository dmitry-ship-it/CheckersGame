using System.Drawing;

namespace CheckersGame.Common.Abstractions
{
    public interface IPlayer
    {
        Color Color { get; init; }

        int CheckersCount { get; set; }
    }
}