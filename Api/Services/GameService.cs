using Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class GameService(QuizLiveDb db)
    {

        public async Task<Game> CreateGame(Game game)
        {
            // generate random game code
            Random random = new Random();
            string gameCode = random.Next(1, 999).ToString();

            // While game code exists, generate new code
            while (db.Games.Any(game => game.GameCode == gameCode))
            {
                gameCode = random.Next(1, 999).ToString();
            }

            game.GameCode = gameCode;
            db.Games.Add(game);
            await db.SaveChangesAsync();
            return game;
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

        public async Task<Game?> StartGame(int gameId)
        {
            // Find game
            var game = await db.Games.FindAsync(gameId);
            // Make sure it's in Waiting state
            if (game is null) return null;
            if (game.Status != Status.Waiting) return null;

            //Get Questions, Randomize them, and get first question
            IEnumerable<Question> questions = db.Questions.Where(question => question.QuizId == game.QuizId);

            // Make sure there are more than 0 questions
            if (questions.Count() == 0) return null;

            // Make sure there are played in the game
            if (!db.Players.Any(p => p.GameId == game.Id)) return null;

            // Change game status
            game.Status = Status.InProgress;
            // Set current question
            var firstQuestion = questions.First();
            game.CurrentQuestionId = firstQuestion.Id;
            // Save changes
            // etc.
            await db.SaveChangesAsync();
            return game;
        }

        //public async Task<IResult> PlayRound(int gameId)
        //{

        //}
    }
}
