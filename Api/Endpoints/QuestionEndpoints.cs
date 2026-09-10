using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Endpoints;

public class QuestionEndpoints
{
    public static void Map(WebApplication app)
    {
        var questions = app.MapGroup("/questions");
        questions.MapGet("/", async (QuizLiveDb db) =>
        {
            var questions = await db.Questions.ToListAsync();

            return Results.Ok(questions);
        });
        questions.MapGet("/{id}", async (int id, QuizLiveDb db) =>
            await db.Questions.FindAsync(id)
                is Question question
                ? Results.Ok(question)
                : Results.NotFound());


        questions.MapPost("/", async (Question question,QuizLiveDb db) =>
        {
            db.Add(question);
            await db.SaveChangesAsync();
            return Results.Created($"/{question.Id}", question);
        });

        questions.MapPut("/{id}", async (int id, Question inputQuestion, QuizLiveDb db) =>
        {
            var question = await db.Questions.FindAsync(id);
            if (question is null) return Results.NotFound();
            question.Text = inputQuestion.Text;
            question.OptionA = inputQuestion.OptionA;
            question.OptionB = inputQuestion.OptionB;
            question.OptionC = inputQuestion.OptionC;
            question.OptionD = inputQuestion.OptionD;
            question.CorrectOption = inputQuestion.CorrectOption;

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        questions.MapDelete("/{id}", async (int id, QuizLiveDb db) =>
        {
            if (await db.Questions.FindAsync(id) is Question question)
            {
                db.Questions.Remove(question);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }
            return Results.NotFound();
        });
    }
}
