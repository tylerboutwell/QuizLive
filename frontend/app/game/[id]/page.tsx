import { API_URL } from "@/lib/api";
import Waiting from "@/components/game/Waiting";
import InProgress from "@/components/game/InProgress";
import Complete from "@/components/game/Complete";


export default async function Page({
    params,
}: {
    params: Promise<{ id: string }>
}) {
    
    const { id } = await params;

    const res = await fetch(`${API_URL}/games/${id}`);

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
    switch (game.status) {
        case "Waiting":
            return <Waiting game={game} />;

        case "InProgress":
            return <InProgress game={game} />;

        case "Complete":
            return <Complete game={game} />;

        default:
            return (
                <main className="min-h-screen flex items-center justify-center">
                    <h1 className="text-3xl font-bold">
                        Invalid game status
                    </h1>
                </main>
            );
    }
}
