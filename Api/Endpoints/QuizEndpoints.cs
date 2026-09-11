using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Endpoints
{
    public class QuizEndpoints
    {
        public static void Map(WebApplication app)
        {
            var quizzes = app.MapGroup("/quizzes/");

            quizzes.MapGet("/", async (QuizLiveDb db) =>
                {
                    var quizList = await db.Quizzes.ToListAsync();
                    return Results.Ok(quizList);
                });

            quizzes.MapGet("/{id}", async (int id, QuizLiveDb db) =>
                await db.Quizzes.FindAsync(id)
                is Quiz quiz
                ? Results.Ok(quiz)
                : Results.NotFound());

            quizzes.MapPost("/", async (Quiz quiz, QuizLiveDb db) =>
            {
                db.Add(quiz);
                await db.SaveChangesAsync();
                return Results.Created($"/{quiz.Id}", quiz);
            });

            quizzes.MapPut("/{id}", async (int id, Quiz inputQuiz, QuizLiveDb db) =>
            {
                var quiz = await db.Quizzes.FindAsync(id);
                if (quiz is null) return Results.NotFound();
                quiz.Title = inputQuiz.Title;
                quiz.Description = inputQuiz.Description;

                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            quizzes.MapDelete("/{id}", async (int id, QuizLiveDb db) =>
            {
                if (await db.Quizzes.FindAsync(id) is Quiz quiz)
                {
                    db.Quizzes.Remove(quiz);
                    await db.SaveChangesAsync();
                    return Results.NoContent();
                }
                return Results.NotFound();
            });


        }
    }
}
