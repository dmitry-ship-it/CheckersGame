export default function ErrorCard(error: { message: string }) {
  return (
    <div className="h-8 text-red-600 place-self-center">
      {error.message.length !== 0 ? <div className="border-2 border-pink-700 p-3">Error: {error.message}</div> : null}
    </div>
  );
}
