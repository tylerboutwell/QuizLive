using Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints
{
    public class QuizEndpoints
    {
        public static void Map(WebApplication app)
        {
            var quizzes = app.MapGroup("/quizzes/");

            quizzes.MapGet("/", GetQuizzes);
            quizzes.MapGet("/{id}", GetQuiz);
            quizzes.MapPost("/", CreateQuiz);
            quizzes.MapPut("/{id}", UpdateQuiz);
            quizzes.MapDelete("/{id}", DeleteQuiz);

            static async Task<Ok<List<Quiz>>> GetQuizzes(QuizLiveDb db)
                {
                    return TypedResults.Ok(await db.Quizzes.ToListAsync());
                };

            static async Task<Results<Ok<Quiz>, NotFound>> GetQuiz(int id, QuizLiveDb db)
                {
                return await db.Quizzes.FindAsync(id)
                    is Quiz quiz
                    ? TypedResults.Ok(quiz)
                    : TypedResults.NotFound();
                };

            static async Task<Created<Quiz>> CreateQuiz(Quiz quiz, QuizLiveDb db)
            {
                db.Add(quiz);
                await db.SaveChangesAsync();
                return TypedResults.Created($"/{quiz.Id}", quiz);
            };

            static async Task<Results<NotFound, NoContent>> UpdateQuiz(int id, Quiz inputQuiz, QuizLiveDb db)
            {
                var quiz = await db.Quizzes.FindAsync(id);
                if (quiz is null) return TypedResults.NotFound();
                quiz.Title = inputQuiz.Title;
                quiz.Description = inputQuiz.Description;

                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            };

            static async Task<Results<NotFound, NoContent>> DeleteQuiz(int id, QuizLiveDb db)
            {
                if (await db.Quizzes.FindAsync(id) is Quiz quiz)
                {
                    db.Quizzes.Remove(quiz);
                    await db.SaveChangesAsync();
                    return TypedResults.NoContent();
                }
                return TypedResults.NotFound();
            };
        }
    }
}
