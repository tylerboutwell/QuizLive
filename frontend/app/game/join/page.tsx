'use client';
import { useState } from "react";
import { API_URL } from "../../../lib/api";
import { json } from "stream/consumers";

export default function Page() {
    const [gameCode, setGameCode] = useState('')
    const [playerName, setPlayerName] = useState('')

    const handleJoin = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        try {
            const response = await fetch(`${API_URL}/games/join`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    gameCode,
                    playerName
                })
            });
            if (!response.ok) {
                throw new Error(`Response status: ${response.status}`);
            }

            const result = await response.json();
            console.log(result);
        } catch (error) {
            console.error(error);
        }
    };

    return (
        <main className="min-h-screen flex items-center justify-center">
            <div className="text-center space-y-6">
                <h1 className="text-5xl font-bold">Join Game</h1>

                <form onSubmit={handleJoin} >
                    <input type='text'
                        placeholder="Enter your name" value={playerName}
                        onChange={(e) => setPlayerName(e.target.value)}
                        className='bg-neutral-secondary-medium border rounded-lg m-1 p-2' />
                    <input type='text'
                        placeholder="Enter game code" value={gameCode}
                        onChange={(e) => setGameCode(e.target.value)}
                        className='bg-neutral-secondary-medium border rounded-lg m-1 p-2'/>

                    <button type='submit' className='p-2 bg-blue-500 hover:bg-blue-400 border rounded-lg'>Submit</button>
                </form>
            </div>
        </main>
    );
}