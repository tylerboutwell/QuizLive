using Api.Models;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs;

public interface IGameClient
{
    Task PlayerJoined(Player player);
}

public class GameHub : Hub<IGameClient>
{
    public async Task JoinGame(int gameId)
    {
        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            $"game-{gameId}"
        );
    }
}