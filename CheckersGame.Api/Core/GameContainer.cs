using CheckersGame.Api.Models;
using CheckersGame.Common;
using CheckersGame.Common.Core;

namespace CheckersGame.Api.Core
{
    public class GameContainer
    {
        private readonly BaseGame _game;

        public GameContainer(BaseGame game, PlayerInfo firstPlayer, PlayerInfo secondPlayer)
        {
            _game = game;
            Players = (firstPlayer, secondPlayer);

            CurrentPlayerTurn = Players.First;

            PushTurnToNextPlayer();
        }

        //public static GameKeysVault Games { get; } = new();

        public Guid GameId { get; } = Guid.NewGuid();

        public PlayerInfo CurrentPlayerTurn { get; private set; }

        public (PlayerInfo First, PlayerInfo Second) Players { get; set; }

        public BaseBoard Board => _game.Board;

        public bool IsEnded => _game.IsEnded;

        public string GameType => _game.GameType.ToString();

        public void NextTurn(Cell from, Cell to)
        {
            _game.NextTurn(from, to);
            PushTurnToNextPlayer();
        }

        public GameModel CreateViewModel(
            Guid playerId,
            Guid? gameId = null,
            string? firstPlayerName = null,
            string? secondPlayerName = null,
            IEnumerable<string?>? board = null,
            string? currentPlayerTurn = null,
            bool? isEnded = null)
        {
            return new GameModel
            {
                Id = gameId ?? GameId,
                PlayerId = playerId,
                FirstPlayerName = firstPlayerName ?? Players.First.Name,
                SecondPlayerName = secondPlayerName ?? Players.Second.Name,
                Board = board ?? Board,
                CurrentPlayerTurn = currentPlayerTurn ?? CurrentPlayerTurn.Id.ToString(),
                IsEnded = isEnded ?? IsEnded
            };
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Players.First.Name, Players.Second.Name, Board);
        }

        private void PushTurnToNextPlayer()
        {
            CurrentPlayerTurn = _game.Players.First == _game.CurrentPlayerTurn
                ? Players.First
                : Players.Second;
        }
    }
}