'use client';

import { useState, useEffect } from "react";
import { API_URL } from "@/lib/api";
import { createConnection } from "@/lib/signalr";
import { Player } from "@/types/player";
import { Game } from "@/types/game";
import { useRouter } from "next/navigation";
import { Question } from "../../types/question";

type InProgressProps = {
    game: Game;
};
export default function InProgress({ game }: InProgressProps) {
    const [players, setPlayers] = useState<Player[]>([]);
    const [playerId, setPlayerId] = useState<number | null>(null)
    const [question, setQuestion] = useState<Question | null>(null);
    const router = useRouter();

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
        const getQuestion = async () => {
            const response = await fetch(
                `${API_URL}/games/${game.id}/question`
            );

            if (!response.ok) {
                throw new Error(`Response status: ${response.status}`);
            }

            const question = await response.json();

            setQuestion(question);
        };

        getQuestion();
    }, [game.id, game.currentQuestionId]);

    useEffect(() => {
        const connection = createConnection();
        let cancelled = false;

        connection.on("PlayerJoined", (player: Player) => {
            setPlayers(prev =>
                prev.some(p => p.id === player.id) ? prev : [...prev, player]
            );
        });

        connection.on("GameStarted", (game: Game) => {
            router.push(`/game/${game.id}`);
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

    const handleStart = async (e: React.SubmitEvent<HTMLFormElement>) => {
        e.preventDefault();

        const response = await fetch(`${API_URL}/games/${game.id}/start`, {
            method: "POST",
        });
        if (!response.ok) {
            throw new Error(`Response status: ${response.status}`);
        }
        const result = await response.json();
        router.push(`/game/${result.id}`);
    }
    return (
        <div className="min-h-screen flex flex-col items-center justify-center gap-4">
            <div className="border rounded-lg p-8 w-80 shadow-sm">
                {/* Question */}
                <div className="text-2xl font-bold">
                    {question?.text}
                </div>

                {/* Options */}
                <div className="mt-4 flex flex-col gap-2">
                    <button className="border rounded-lg p-3">
                        {question?.optionA}
                    </button>

                    <button className="border rounded-lg p-3">
                        {question?.optionB}
                    </button>

                    <button className="border rounded-lg p-3">
                        {question?.optionC}
                    </button>

                    <button className="border rounded-lg p-3">
                        {question?.optionD}
                    </button>
                </div>


                {/* Scoreboard */}
                <div className="mt-6">
                    <div className="font-bold mb-2">
                        Scoreboard
                    </div>

                    <div className="flex flex-col gap-2">
                        {players
                            .sort((a, b) => b.score - a.score)
                            .map((player, index) => (
                                <div
                                    key={player.id}
                                    className="flex justify-between border rounded-lg p-2"
                                >
                                    <span>
                                        {index + 1}. {player.name}
                                    </span>

                                    <span className="font-bold">
                                        {player.score}
                                    </span>
                                </div>
                            ))}
                    </div>
                </div>

            </div>
        </div>
    );
}