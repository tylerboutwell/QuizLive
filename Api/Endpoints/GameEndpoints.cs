using Microsoft.AspNetCore.Http.HttpResults;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints
{
    public class GameEndpoints
    {
        public static void Map(WebApplication app)
        {
            var games = app.MapGroup("/games");

            games.MapGet("/", GetGames);
            games.MapGet("/{id}", GetGame);
            games.MapPost("/", CreateGame);
            games.MapPut("/{id}", UpdateGame);
            games.MapDelete("/{id}", DeleteGame);


            static async Task<Ok<List<Game>>> GetGames(QuizLiveDb db)
            {
                return TypedResults.Ok(await db.Games.ToListAsync());
            };

            static async Task<Results<Ok<Game>,NotFound>> GetGame(QuizLiveDb db, int id)
            {
                var game = await db.Games.FindAsync(id);
                if (game is null) return TypedResults.NotFound();
                return TypedResults.Ok(game);
            };

            static async Task<Created<Game>> CreateGame(QuizLiveDb db, Game game)
            {
                db.Games.Add(game);
                await db.SaveChangesAsync();
                return TypedResults.Created("/{game.id}", game);
            };

            static async Task<Results<NotFound, NoContent>> UpdateGame(QuizLiveDb db, Game inputGame, int id)
            {
                var game = await db.Games.FindAsync(id);
                if (game is null) return TypedResults.NotFound();
                game.GameCode = inputGame.GameCode;
                game.Status = inputGame.Status;

                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            };

            static async Task<Results<NotFound, NoContent>> DeleteGame(QuizLiveDb db, int id)
            {
                var game = await db.Games.FindAsync(id);
                if (game is null) return TypedResults.NotFound();
                db.Games.Remove(game);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }
        }
    }
}
