using System.Drawing;

namespace CheckersGame.Common.Core
{
    public class BasePlayer
    {
        public BasePlayer(Color color, int checkersCount)
        {
            Color = color;
            CheckersCount = checkersCount;
        }

        public Color Color { get; init; }
        public int CheckersCount { get; set; }
    }
}