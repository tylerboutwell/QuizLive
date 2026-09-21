export default function Home() {
  return (
    <main className="min-h-screen flex items-center justify-center">
      <div className="text-center space-y-6">
        <h1 className="text-5xl font-bold">QuizLive</h1>

        <p className="text-gray-600">
          A live quiz game.
        </p>

        <div className="flex gap-4 justify-center">
          <button className="px-6 py-3 rounded-lg bg-black text-white">
            Create Game
          </button>

          <button className="px-6 py-3 rounded-lg border border-gray-300">
            Join Game
          </button>
        </div>
      </div>
    </main>
  );
}