using Microsoft.EntityFrameworkCore;
using Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Endpoints;

public class QuestionEndpoints
{
    public static void Map(WebApplication app)
    {
        var questions = app.MapGroup("/questions");
        questions.MapGet("/", async Task<Ok<List<Question>>> (QuizLiveDb db) =>
        {
            var questions = await db.Questions.ToListAsync();

            return TypedResults.Ok(questions);
        });

        questions.MapGet("/{id}", async Task<Results<Ok<Question>, NotFound>> (int id, QuizLiveDb db) =>
            await db.Questions.FindAsync(id)
                is Question question
                ? TypedResults.Ok(question)
                : TypedResults.NotFound());


        questions.MapPost("/", async Task<Created<Question>> (Question question,QuizLiveDb db) =>
        {
            db.Add(question);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/{question.Id}", question);
        });

        questions.MapPut("/{id}", async Task<Results<NotFound, NoContent>> (int id, Question inputQuestion, QuizLiveDb db) =>
        {
            var question = await db.Questions.FindAsync(id);
            if (question is null) return TypedResults.NotFound();
            question.Text = inputQuestion.Text;
            question.OptionA = inputQuestion.OptionA;
            question.OptionB = inputQuestion.OptionB;
            question.OptionC = inputQuestion.OptionC;
            question.OptionD = inputQuestion.OptionD;
            question.CorrectOption = inputQuestion.CorrectOption;

            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        });

        questions.MapDelete("/{id}", async Task<Results<NoContent, NotFound>> (int id, QuizLiveDb db) =>
        {
            if (await db.Questions.FindAsync(id) is Question question)
            {
                db.Questions.Remove(question);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }
            return TypedResults.NotFound();
        });
    }
}
