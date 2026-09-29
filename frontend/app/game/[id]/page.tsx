import { API_URL } from "../../../lib/api";

export default async function Page({
    params,
}: {
    params: Promise<{ id: string }>
}) {
    
    const { id } = await params;

    const res = await fetch(`${API_URL}/games/${id}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        },
    })
    if (!res.ok) {
        return (
            <main className="min-h-screen flex items-center justify-center">
                <h1 className="text-3xl font-bold">
                    Game does not exist
                </h1>
            </main>
        );
    }
    const game = await res.json();
    return (
        <main className="min-h-screen flex items-center justify-center">
            <div>
                <h1 className="text-3xl font-bold">Game Status - {game.status}</h1>
            </div>
        </main>
    );
}
