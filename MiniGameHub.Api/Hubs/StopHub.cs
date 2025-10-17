using Microsoft.AspNetCore.SignalR;

namespace MiniGameHub.Api.Hubs;

public class StopHub : Hub
{
    public async Task JoinSessionGroup(string sessionId)
    {
        await Clients.Group(sessionId).SendAsync(Context.ConnectionId, sessionId);
    }
}