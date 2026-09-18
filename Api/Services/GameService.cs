using Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Services
{
    public class GameService(QuizLiveDb db)
    {

        public async Task<Game> CreateGame(Game game)
        {
            db.Games.Add(game);
            await db.SaveChangesAsync();
            return game;
        }

        public async Task<IResult> StartGame(int gameId)
        {
            // Find game
            var game = await db.Games.FindAsync(gameId);
            // Make sure it's in Waiting state
            if (game is null) return TypedResults.NotFound();
            if (game.Status.ToString() != "Waiting") return TypedResults.BadRequest();

            //Get Questions, Randomize them, and get first question
            IEnumerable<Question> questions = db.Questions.Where(question => question.Id == game.QuizId);

            // Make sure there are more than 0 questions
            if (questions.Count() == 0) return TypedResults.BadRequest();

            // Change game status
            game.Status = Status.inProgress;
            // Set current question
            var firstQuestion = questions.First();
            game.CurrentQuestionId = firstQuestion.Id;
            // Save changes
            // etc.
            await db.SaveChangesAsync();
            return TypedResults.Ok(game);
        }

        public async Task<IResult> PlayRound(int gameId)
        {

        }
    }
}
