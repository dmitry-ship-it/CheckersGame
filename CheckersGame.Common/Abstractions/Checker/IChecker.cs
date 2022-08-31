using System.Drawing;

namespace CheckersGame.Common.Abstractions.Checker
{
    public interface IChecker : ICloneable
    {
        IMoveset Moveset { get; set; }

        Color Color { get; init; }
    }
}