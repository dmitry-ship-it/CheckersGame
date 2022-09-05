using CheckersGame.Api.Models;
using CheckersGame.Common;
using CheckersGame.Common.Abstractions;

namespace CheckersGame.Api.Core
{
    public class GameContainer
    {
        public Guid GameId { get; } = Guid.NewGuid();

        public Guid PlayerTurnId { get; private set; }

        public PlayerInfo FirstPlayer { get; set; }
        public PlayerInfo SecondPlayer { get; set; }

        public IGame Game { get; init; }

        public bool IsEnded => Game.EndMessage is null;

        public void MoveGameByContainer(Cell from, Cell to)
        {
            Game.NextTurn(from, to);
            PushTurnToNextPlayer();
        }

        public GameContainer(IGame game, PlayerInfo firstPlayer, PlayerInfo secondPlayer)
        {
            Game = game;
            FirstPlayer = firstPlayer;
            SecondPlayer = secondPlayer;

            PushTurnToNextPlayer();
        }

        private void PushTurnToNextPlayer()
        {
            PlayerTurnId = Game.Players.First == Game.CurrentPlayerTurn
                ? FirstPlayer.Id
                : SecondPlayer.Id;
        }
    }
}
