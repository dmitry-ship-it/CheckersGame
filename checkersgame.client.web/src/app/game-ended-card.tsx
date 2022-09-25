export default function GameEndedCard() {
  return (
    <div className="w-full h-full top-0 left-0 bg-black bg-opacity-90 absolute">
      <div className="h-1/2 w-1/2 absolute left-1/4 top-1/4 rounded-xl text-white bg-gradient-to-br from-cyan-500 via-purple-500 to-emerald-500">
        <div className="min-h-full min-w-full flex flex-col justify-between text-center">
          <div className="text-3xl flex justify-center mt-20">Game Ended!</div>
          <a className="place-self-center bg-blue-700 rounded-xl mb-20 p-1 w-20 text-xl shadow-lg" href="/">
            Exit
          </a>
        </div>
      </div>
    </div>
  );
}
