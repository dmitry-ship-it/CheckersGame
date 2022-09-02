using CheckersGame.Api.Core;
using CheckersGame.Common.Abstractions;
using CheckersGame.Common.Impl.International;

namespace CheckersGame.Common
{
    public class GameFactory
    {
        public IGame CreateNew(GameType gameType)
        {
            return gameType switch
            {
                GameType.International => new InternationalGame(),
                _ => throw new ArgumentException("Unknown game type.", nameof(gameType)),
            };
        }
    }
}
