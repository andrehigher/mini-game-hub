namespace MiniGameHub.Api.Models;

public class GameSession
{
    public int Id { get; set; }
    public string SessionName { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<Player> Players { get; set; } = new();
    public List<Round> Rounds { get; set; } = new();
}