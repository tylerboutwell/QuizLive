using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs;

public class GameHub : Hub
{
    public async Task JoinGame(string gameCode, string playerName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, gameCode);

        await Clients.Group(gameCode).SendAsync(
            "PlayerJoined",
            playerName
        );
    }
}