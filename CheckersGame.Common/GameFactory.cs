using CheckersGame.Api.Core;
using CheckersGame.Common.Core;
using CheckersGame.Common.Impl.International;

namespace CheckersGame.Common
{
    public class GameFactory
    {
        public BaseGame CreateNew(GameType gameType)
        {
            return gameType switch
            {
                GameType.International => new BaseGame(GameType.International, GameRules.International),
                GameType.Default => new BaseGame(GameType.Default, GameRules.Default),
                _ => throw new ArgumentException("Unknown game type.", nameof(gameType)),
            };
        }

        public BaseGame CreateNew(GameRules gameRules)
        {
            return new BaseGame(GameType.Custom, gameRules);
        }
    }
}
