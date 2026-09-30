import { Game } from "@/types/game";

type InProgressProps = {
    game: Game;
};
export default function InProgress({ game }: InProgressProps) {


    return (
        <div className="min-h-screen flex items-center justify-center">

            <h1 className="text-3xl font-bold">Game in progress! - {game.status}</h1>

        </div>
    )
}