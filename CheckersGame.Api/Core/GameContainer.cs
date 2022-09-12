using CheckersGame.Api.Models;
using CheckersGame.Common;
using CheckersGame.Common.Abstractions;
using CheckersGame.Common.Abstractions.Board;

namespace CheckersGame.Api.Core
{
    // TODO: Rename with 'GameManager'. Also rename all variable names of this type.
    public class GameContainer
    {
        private readonly IGame _game;

        public GameContainer(IGame game, PlayerInfo firstPlayer, PlayerInfo secondPlayer)
        {
            _game = game;
            FirstPlayer = firstPlayer;
            SecondPlayer = secondPlayer;

            CurrentPlayerTurn = FirstPlayer;

            PushTurnToNextPlayer();
        }

        public Guid GameId { get; } = Guid.NewGuid();

        public PlayerInfo CurrentPlayerTurn { get; private set; }

        public PlayerInfo FirstPlayer { get; set; }
        public PlayerInfo SecondPlayer { get; set; }

        public IBoard Board => _game.Board;

        public bool IsEnded => _game.EndMessage is null;

        // TODO: Replace implementation with GameType enum name
        public string GameType =>
            _game.GetType().Name.Replace("Game", string.Empty);

        public void NextTurn(Cell from, Cell to)
        {
            _game.NextTurn(from, to);
            PushTurnToNextPlayer();
        }

        public PlayerInfo GetEnemyForPlayer(Guid enemyPlayerId)
        {
            if (FirstPlayer.Id == enemyPlayerId)
            {
                return SecondPlayer;
            }
            else if (SecondPlayer.Id == enemyPlayerId)
            {
                return FirstPlayer;
            }
            else
            {
                throw new ArgumentException("There is no player with this id.", nameof(enemyPlayerId));
            }
        }
        // TODO: Merge this two methods --^ --v
        public PlayerInfo GetPlayerById(Guid id)
        {
            if (FirstPlayer.Id == id)
            {
                return FirstPlayer;
            }
            else if (SecondPlayer.Id == id)
            {
                return SecondPlayer;
            }
            else
            {
                throw new ArgumentException("There is no player with this id.", nameof(id));
            }
        }

        private void PushTurnToNextPlayer()
        {
            CurrentPlayerTurn = _game.Players.First == _game.CurrentPlayerTurn
                ? FirstPlayer
                : SecondPlayer;
        }
    }
}
