using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints
{
    public class PlayerEndpoints
    {
        public static void Map(WebApplication app)
        {
            var questions = app.MapGroup("/players");
            questions.MapGet("/", async (QuizLiveDb db) =>
            {
                var questions = await db.Players.ToListAsync();

                return Results.Ok(questions);
            });
            questions.MapGet("/{id}", async (int id, QuizLiveDb db) =>
                await db.Players.FindAsync(id)
                    is Player player
                    ? Results.Ok(player)
                    : Results.NotFound());


            questions.MapPost("/", async (Player player, QuizLiveDb db) =>
            {
                db.Add(player);
                await db.SaveChangesAsync();
                return Results.Created($"/{player.Id}", player);
            });

            questions.MapPut("/{id}", async (int id, Player inputPlayer, QuizLiveDb db) =>
            {
                var player = await db.Players.FindAsync(id);
                if (player is null) return Results.NotFound();
                player.Name = inputPlayer.Name;
                player.Score = inputPlayer.Score;

                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            questions.MapDelete("/{id}", async (int id, QuizLiveDb db) =>
            {
                if (await db.Players.FindAsync(id) is Player player)
                {
                    db.Players.Remove(player);
                    await db.SaveChangesAsync();
                    return Results.NoContent();
                }
                return Results.NotFound();
            });
        }
    }
}
