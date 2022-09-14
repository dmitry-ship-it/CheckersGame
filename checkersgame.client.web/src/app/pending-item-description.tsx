export default function PendingItemDescription(gameInfo: { gameType: string; firstPlayerName: string }) {
  return (
    <div className="mr-20">
      <div>Game type: {gameInfo.gameType}</div>
      <div>
        <span className="text-white font-semibold">{gameInfo.firstPlayerName}</span> is waiting for opponent
      </div>
    </div>
  );
}
