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

            quizzes.MapGet("/", async Task<Ok<List<Quiz>>> (QuizLiveDb db) =>
                {
                    return TypedResults.Ok(await db.Quizzes.ToListAsync());
                });

            quizzes.MapGet("/{id}", async Task<Results<Ok<Quiz>, NotFound>> (int id, QuizLiveDb db) =>
                await db.Quizzes.FindAsync(id)
                is Quiz quiz
                ? TypedResults.Ok(quiz)
                : TypedResults.NotFound());

            quizzes.MapPost("/", async Task<Created<Quiz>> (Quiz quiz, QuizLiveDb db) =>
            {
                db.Add(quiz);
                await db.SaveChangesAsync();
                return TypedResults.Created($"/{quiz.Id}", quiz);
            });

            quizzes.MapPut("/{id}", async Task<Results<NotFound, NoContent>> (int id, Quiz inputQuiz, QuizLiveDb db) =>
            {
                var quiz = await db.Quizzes.FindAsync(id);
                if (quiz is null) return TypedResults.NotFound();
                quiz.Title = inputQuiz.Title;
                quiz.Description = inputQuiz.Description;

                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            });

            quizzes.MapDelete("/{id}", async Task<Results<NotFound, NoContent>> (int id, QuizLiveDb db) =>
            {
                if (await db.Quizzes.FindAsync(id) is Quiz quiz)
                {
                    db.Quizzes.Remove(quiz);
                    await db.SaveChangesAsync();
                    return TypedResults.NoContent();
                }
                return TypedResults.NotFound();
            });


        }
    }
}
