using Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class GameService(QuizLiveDb db)
    {

        public async Task<(Game game, Player player)> CreateGame(int quizId, string playerName)
        {
            // generate random game code
            Random random = new Random();
            string gameCode = random.Next(1, 999).ToString();

            // While game code exists, generate new code
            while (db.Games.Any(game => game.GameCode == gameCode))
            {
                gameCode = random.Next(1, 999).ToString();
            }
            Game game = new Game
            {
                GameCode = gameCode,
                QuizId = quizId
            };
            db.Games.Add(game);
            await db.SaveChangesAsync();

            Player player = new Player
            {
                Name = playerName,
                IsHost = true,
                GameId = game.Id
            };
            db.Players.Add(player);
            await db.SaveChangesAsync();
            return (game, player);
        }

        public async Task<Player?> JoinGame(string gameCode, string playerName)
        {
            var game = await db.Games.FirstOrDefaultAsync(g => g.GameCode == gameCode);
            //Check if game exists
            if (game is null) return null;

            var player = new Player
            {
                Name = playerName,
                GameId = game.Id
            };

            db.Players.Add(player);
           

            await db.SaveChangesAsync();
            return player;

        }

        public async Task<Game> StartGame(int gameId)
        {
            // Find game
            var game = await db.Games.FindAsync(gameId);
            // Make sure game exists and is in waiting status
            if (game is null) throw new Exception("Game not found");
            if (game.Status != Status.Waiting) throw new Exception("Game is not in waiting status");

            //Get Questions
            IEnumerable<Question> questions = db.Questions.Where(question => question.QuizId == game.QuizId);

            // Make sure there are more than 0 questions
            if (!questions.Any()) throw new Exception("No questions available for the quiz");

            // Make sure there are players in the game
            if (!db.Players.Any(p => p.GameId == game.Id)) throw new Exception("No players in the game");

            // Change game status
            game.Status = Status.InProgress;

            // Get a random question from the list of questions and set it as the current question
            var firstQuestion = questions
                .OrderBy(q => Guid.NewGuid())
                .First();
            game.CurrentQuestionId = firstQuestion.Id;
            // Save changes
            // etc.
            await db.SaveChangesAsync();
            return game;
        }

        public async Task<SubmitAnswerResult?> SubmitAnswer(int playerId, string answer)
        {
            var player = await db.Players.FindAsync(playerId);
            if (player is null) return null;

            var game = await db.Games.FindAsync(player.GameId);
            if (game is null) return null;

            var question = await db.Questions.FindAsync(game.CurrentQuestionId);
            if (question is null) return null;

            var isCorrect = question.CorrectOption == answer;
            if (isCorrect)
            {
                player.Score += 1;
                await db.SaveChangesAsync();
            };

            return new SubmitAnswerResult
            { 
                IsCorrect = isCorrect,
                Players = await db.Players.Where(p => p.GameId == game.Id).ToListAsync()
            };

        }
    }
}
