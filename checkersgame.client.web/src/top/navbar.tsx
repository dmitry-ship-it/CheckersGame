let playerName: string = "";

function Navbar() {
  return (
    <div className="sticky bg-gray-500 m-auto py-3">
      <nav className="container w-screen mx-auto flex flex-wrap justify-between">
        <a className="text-3xl font-bold" href="/">
          Checkers game
        </a>
        <input
          className="text-xl w-32 text-center rounded-xl"
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

export default Navbar;
export { playerName };
