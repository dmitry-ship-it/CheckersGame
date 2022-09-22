export default function UserCard(user: { name: string; isMoving: boolean }) {
  const border = user.isMoving ? "border-fuchsia-800" : "border-transparent";

  return (
    <div className={`px-4 py-2 rounded-xl bg-slate-400 border-4 ${border}`}>
      <div className="">Username:</div>
      <div className="inline-block overflow-hidden overflow-ellipsis w-32 font-bold">{user.name}</div>
    </div>
  );
}
