using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MiniGameHub.Api.Data;
using MiniGameHub.Api.Dtos;
using MiniGameHub.Api.Hubs;
using MiniGameHub.Api.Models;

namespace MiniGameHub.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GameSessionController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IHubContext<StopHub> _hubContext;
    
    public GameSessionController(AppDbContext dbContext, IHubContext<StopHub> hubContext)
    {
        _dbContext = dbContext;
        _hubContext = hubContext;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateSession([FromBody] CreateSessionRequest request)
    {
        var player = new Player
        {
            Name = request.PlayerName,
            Email = $"{Guid.NewGuid()}@minigamehub.com" 
        };
        
        var session = new GameSession
        {
            SessionName = request.SessionName,
            Players = new List<Player> { player }
        };

        await _dbContext.GameSessions.AddAsync(session);
        await _dbContext.SaveChangesAsync();
        
        return Ok(new { SessionId = session.Id, PlayerId = player.Id });
    }
    
    [HttpPost("join")]
    public async Task<IActionResult> JoinSession([FromBody] JoinSessionRequest request)
    {
        var session = await _dbContext.GameSessions.FindAsync(request.SessionId);
        if (session == null)
        {
            return NotFound("Session not found");
        }

        var player = new Player
        {
            Name = request.PlayerName,
            Email = $"{Guid.NewGuid()}@minigamehub.com"
        };

        session.Players.Add(player);
        await _dbContext.SaveChangesAsync();
        
        await _hubContext.Clients.Group(request.SessionId.ToString())
            .SendAsync("PlayerJoined", $"{player.Name} entrou na sessão.");

        return Ok(new { PlayerId = player.Id });
    }
}