import UserCard from "./game-user-card";
import { Game } from "./utils/types";

let isFirstPlayerMoving = false;

export default function GameInfoColumn(ctx: { game: Game }) {
  isFirstPlayerMoving = !isFirstPlayerMoving;
  return (
    <div className="flex flex-col justify-around max-w-xl pr-10">
      <UserCard name={ctx.game.secondPlayerName} isMoving={isFirstPlayerMoving} />
      <UserCard name={ctx.game.firstPlayerName} isMoving={!isFirstPlayerMoving} />
    </div>
  );
}
