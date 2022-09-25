using CheckersGame.Api.Models;
using CheckersGame.Common;
using CheckersGame.Common.Core;

namespace CheckersGame.Api.Core
{
    public class GameContainer
    {
        private readonly BaseGame _game;

        // this field is accessed via methods to make it thread-safe
        private static readonly SortedDictionary<Guid, bool> _pendingGames = new();

        public GameContainer(BaseGame game, PlayerInfo firstPlayer, PlayerInfo secondPlayer)
        {
            _game = game;
            Players = (firstPlayer, secondPlayer);

            CurrentPlayerTurn = Players.First;

            PushTurnToNextPlayer();
        }

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

        public GameModel CreateGameModel(
            Guid playerId,
            Guid? gameId = null,
            string? firstPlayerId = null,
            string? secondPlayerId = null,
            IEnumerable<string?>? board = null,
            string? currentPlayerTurn = null,
            bool? isEnded = null)
        {
            return new GameModel
            {
                Id = gameId ?? GameId,
                PlayerId = playerId,
                FirstPlayerName = firstPlayerId ?? Players.First.Name,
                SecondPlayerName = secondPlayerId ?? Players.Second.Name,
                Board = board ?? Board,
                CurrentPlayerTurn = currentPlayerTurn ?? CurrentPlayerTurn.Id.ToString(),
                IsEnded = isEnded ?? IsEnded
            };
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Players.First.Name, Players.Second.Name, Board);
        }

        public static void SetGameStatus(Guid id, bool isPending)
        {
            lock (_pendingGames)
            {
                _pendingGames[id] = isPending;
            }
        }

        public static bool RemoveGame(Guid id)
        {
            lock (_pendingGames)
            {
                return _pendingGames.Remove(id);
            }
        }

        public static bool IsGamePending(Guid id)
        {
            return _pendingGames[id];
        }

        public static int GetGamesCount()
        {
            return _pendingGames.Count;
        }

        public static IEnumerable<KeyValuePair<Guid, bool>> GetAllGamesStatus()
        {
            // create copy to avoid concurrency
            return _pendingGames.ToArray();
        }

        public static bool IsGameExists(Guid id)
        {
            return _pendingGames.ContainsKey(id);
        }

        private void PushTurnToNextPlayer()
        {
            CurrentPlayerTurn = _game.Players.First == _game.CurrentPlayerTurn
                ? Players.First
                : Players.Second;
        }
    }
}