export let playerName: string = "";

export default function Navbar() {
  return (
    <div className="sticky py-3 dark:bg-stone-600 bg-slate-400">
      <nav className="container w-screen mx-auto flex flex-wrap justify-between">
        <a className="text-3xl font-bold dark:text-white" href="/">
          Checkers game
        </a>
        <input
          className="text-xl max-w-sm p-full leading-none text-center rounded-xl select-none"
          type="text"
          name="playerName"
          placeholder="player name"
          onChange={(e) => {
            playerName = e.target.value;
          }}
        />
      </nav>
    </div>
  );
}
