using Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints
{
    public class PlayerEndpoints
    {
        public static void Map(WebApplication app)
        {
            var questions = app.MapGroup("/players");
            questions.MapGet("/", async Task<Ok<List<Player>>> (QuizLiveDb db) =>
            {
                var questions = await db.Players.ToListAsync();

                return TypedResults.Ok(questions);
            });
            questions.MapGet("/{id}", async Task<Results<Ok<Player>, NotFound>> (int id, QuizLiveDb db) =>
                await db.Players.FindAsync(id)
                    is Player player
                    ? TypedResults.Ok(player)
                    : TypedResults.NotFound());


            questions.MapPost("/", async Task<Created<Player>> (Player player, QuizLiveDb db) =>
            {
                db.Add(player);
                await db.SaveChangesAsync();
                return TypedResults.Created($"/{player.Id}", player);
            });

            questions.MapPut("/{id}", async Task<Results<NotFound, NoContent>> (int id, Player inputPlayer, QuizLiveDb db) =>
            {
                var player = await db.Players.FindAsync(id);
                if (player is null) return TypedResults.NotFound();
                player.Name = inputPlayer.Name;
                player.Score = inputPlayer.Score;

                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            });

            questions.MapDelete("/{id}", async Task<Results<NoContent, NotFound>> (int id, QuizLiveDb db) =>
            {
                if (await db.Players.FindAsync(id) is Player player)
                {
                    db.Players.Remove(player);
                    await db.SaveChangesAsync();
                    return TypedResults.NoContent();
                }
                return TypedResults.NotFound();
            });
        }
    }
}
