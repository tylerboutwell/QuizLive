'use client';

import { useState, useEffect } from "react";
import { API_URL } from "@/lib/api";
import { createConnection } from "@/lib/signalr";
import { Player } from "@/types/player";
import { Game } from "@/types/game";

type WaitingProps = {
    game: Game;
};
export default function Waiting({ game }: WaitingProps) {
    const [players, setPlayers] = useState<Player[]>([]);
    const [playerId, setPlayerId] = useState<number | null>(null)

    useEffect(() => {
        const getPlayers = async () => {
            const storedPlayerId = localStorage.getItem("playerId");
            setPlayerId(storedPlayerId ? Number(storedPlayerId) : null);
            const response = await fetch(`${API_URL}/games/${game.id}/players`);
            const data = await response.json();
            setPlayers(data);
        };

        getPlayers();
    }, [game.id]);

    useEffect(() => {
        const connection = createConnection();
        let cancelled = false;

        connection.on("PlayerJoined", (player: Player) => {
            setPlayers(prev =>
                prev.some(p => p.id === player.id) ? prev : [...prev, player]
            );
        });

        connection.onreconnected(() => {
            connection.invoke("JoinGame", game.id).catch(console.error);
        });

        const startConnection = async () => {
            try {
                await connection.start();
                if (cancelled) return;

                await connection.invoke("JoinGame", game.id);
                console.log("SignalR connected and joined game!");
            } catch (err) {
                if (cancelled) return; // expected in dev from Strict Mode
                console.error("SignalR error:", err);
            }
        };

        startConnection();

        return () => {
            cancelled = true;
            connection.stop();
        };
    }, [game.id]);

    const currentPlayer = players.find(
        player => player.id === playerId
    );
    return (
        <div className="min-h-screen flex flex-col items-center justify-center gap-4">
            <div className="border rounded-lg p-8 w-80 shadow-sm">

            <div className="text-2xl font-bold">
                Waiting for players
            </div>

            <div className="text-gray-600">
                Game Code: {game.gameCode}
            </div>

            <div className="mt-4 text-lg">
                Players
            </div>

            <ul className="text-gray-700 text-center">
                {players.map(player => (
                    <li key={player.id}>
                        {player.name}
                    </li>
                ))}
                </ul>
                {currentPlayer?.isHost && (
                    <button className="mt-4 px-4 py-2 rounded bg-black text-white">
                        Start Game
                    </button>
                )}
            </div>
        </div>
    )
}