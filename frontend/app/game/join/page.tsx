export default function Page() {
    return (
        <main className="min-h-screen flex items-center justify-center">
            <div className="text-center space-y-6">
                <h1 className="text-5xl font-bold">Join Game</h1>

                <p className="text-gray-600">
                    Enter game code: <input className='bg-neutral-secondary-medium border rounded-lg p-2'></input>
                </p>
            </div>
        </main>
    );
}