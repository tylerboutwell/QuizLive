import { Game } from "@/types/game";

type WaitingProps = {
    game: Game;
};
export default function Waiting({ game }: WaitingProps) {


    return (
        <div className="min-h-screen flex items-center justify-center">
            
            <h1 className="text-3xl font-bold">Waiting for players - {game.status}</h1>

        </div>
    )
}