import { useEffect, useState } from "react";
import ApiRouter from "./utils/router";

export let SelectedGameType: string;

export default function GameTypesDropdown() {
  const [gameTypes, setGameTypes] = useState<string[]>([]);
  useEffect(() => {
    const setTypes = async () => {
      const data: string[] = await ApiRouter.get("types");
      SelectedGameType = data[0];
      setGameTypes(data);
    };
    setTypes();
  }, []);

  return (
    <select
      className="mx-3 py-2 px-1 rounded-lg"
      onChange={(e) => {
        SelectedGameType = e.currentTarget.value;
      }}>
      {gameTypes.map((gameType) => {
        return <option value={gameType}>{gameType}</option>;
      })}
    </select>
  );
}
