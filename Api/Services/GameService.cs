using Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Services
{
    public class GameService(QuizLiveDb db)
    {

        public async Task<Game> CreateGame(Game game)
        {
            db.Games.Add(game);
            await db.SaveChangesAsync();
            return game;
        }

        //public async Task<Game> StartGame(int gameId)
        //{
            // Find game
            // Make sure it's in Waiting state
            // Find first question
            // Change game status
            // Set current question
            // Save changes
            // etc.
        //}
    }
}
