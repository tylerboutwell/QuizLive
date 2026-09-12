using Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints
{
    public class PlayerEndpoints
    {
        public static void Map(WebApplication app)
        {
            var players = app.MapGroup("/players");

            players.MapGet("/", GetPlayers);
            players.MapGet("/{id}", GetPlayer);
            players.MapPost("/", CreatePlayer);
            players.MapPut("/{id}", UpdatePlayer);
            players.MapDelete("/{id}", DeletePlayer);

            static async Task<Ok<List<Player>>> GetPlayers(QuizLiveDb db)
            {
                var players = await db.Players.ToListAsync();

                return TypedResults.Ok(players);
            };

            static async Task<Results<Ok<Player>, NotFound>> GetPlayer(int id, QuizLiveDb db)
            {
            return await db.Players.FindAsync(id)
                is Player player
                ? TypedResults.Ok(player)
                : TypedResults.NotFound();
            };


            static async Task<Created<Player>> CreatePlayer(Player player, QuizLiveDb db)
            {
                db.Add(player);
                await db.SaveChangesAsync();
                return TypedResults.Created($"/{player.Id}", player);
            };

            static async Task<Results<NotFound, NoContent>> UpdatePlayer(int id, Player inputPlayer, QuizLiveDb db)
            {
                var player = await db.Players.FindAsync(id);
                if (player is null) return TypedResults.NotFound();
                player.Name = inputPlayer.Name;
                player.Score = inputPlayer.Score;

                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            };

            static async Task<Results<NoContent, NotFound>> DeletePlayer(int id, QuizLiveDb db)
            {
                if (await db.Players.FindAsync(id) is Player player)
                {
                    db.Players.Remove(player);
                    await db.SaveChangesAsync();
                    return TypedResults.NoContent();
                }
                return TypedResults.NotFound();
            };
        }
    }
}
