using Microsoft.EntityFrameworkCore;
using Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Api.DTOs;

namespace Api.Endpoints;

public class QuestionEndpoints
{
    public static void Map(WebApplication app)
    {
        var questions = app.MapGroup("/questions");

        questions.MapGet("/", GetQuestions);
        questions.MapGet("/{id}", GetQuestion);
        questions.MapPost("/", CreateQuestion);
        questions.MapPut("/{id}", UpdateQuestion);
        questions.MapDelete("/{id}", DeleteQuestion);

        static async Task<IResult> GetQuestions(QuizLiveDb db)
        {
           return TypedResults.Ok(await db.Questions.Select(x => new QuestionDTO(x)).ToArrayAsync());
        };

        static async Task<IResult> GetQuestion(int id, QuizLiveDb db)
        {
            return await db.Questions.FindAsync(id)
                is Question question
                ? TypedResults.Ok(new QuestionDTO(question))
                : TypedResults.NotFound();
        };

        static async Task<IResult> CreateQuestion(Question question,QuizLiveDb db)
        {
            db.Questions.Add(question);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/questions/{question.Id}", new QuestionDTO(question));
        };

        static async Task<IResult> UpdateQuestion(int id, Question inputQuestion, QuizLiveDb db)
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
        };

        static async Task<Results<NoContent, NotFound>> DeleteQuestion(int id, QuizLiveDb db)
        {
            if (await db.Questions.FindAsync(id) is Question question)
            {
                db.Questions.Remove(question);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }
            return TypedResults.NotFound();
        };
    }
}
