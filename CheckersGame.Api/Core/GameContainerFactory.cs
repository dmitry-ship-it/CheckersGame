using CheckersGame.Common;

namespace CheckersGame.Api.Core
{
    public class GameContainerFactory
    {
        private readonly GameFactory _gameFactory;

        public GameContainerFactory(GameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public GameContainer Create(
            GameType gameType,

            // default players names 
            string firstPlayerName = "",
            string secondPlayerName = "")
        {
            if (string.IsNullOrEmpty(firstPlayerName))
            {
                firstPlayerName = "Noname_first";
            }

            var first = new PlayerInfo(firstPlayerName);
            var second = new PlayerInfo(secondPlayerName);

            return new GameContainer(_gameFactory.CreateNew(gameType), first, second);
        }
    }
}
