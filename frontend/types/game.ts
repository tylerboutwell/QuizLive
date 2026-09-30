export type Game = {
    id: number;
    quizId: number;
    gameCode: string;
    status: string;
    currentQuestionId: number | null;
};