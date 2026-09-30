import { Game } from "@/types/game";

type CompleteProps = {
    game: Game;
};
export default function Complete({ game }: CompleteProps) {


    return (
        <div className="min-h-screen flex items-center justify-center">

            <h1 className="text-3xl font-bold">Game Complete - {game.status}</h1>
        </div>
    )
}