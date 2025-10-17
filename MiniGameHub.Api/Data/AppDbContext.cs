using Microsoft.EntityFrameworkCore;
using MiniGameHub.Api.Models;

namespace MiniGameHub.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Player> Players { get; set; }
    public DbSet<GameSession> GameSessions { get; set; }
    public DbSet<Round> Rounds { get; set; }
    public DbSet<PlayerAnswer> PlayerAnswers { get; set; }
}