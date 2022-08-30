using CheckersGame.Common.Abstractions;
using System.Drawing;

namespace CheckersGame.Common.Impl.General
{
    internal abstract class BasePlayer : IPlayer
    {
        protected BasePlayer(int checkers)
        {
            CheckersCount = checkers;
        }

        public abstract Color Color { get; init; }
        public int CheckersCount { get; set; }
    }
}