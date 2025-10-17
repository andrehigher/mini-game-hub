namespace MiniGameHub.Api.Models;

public class Round
{
    public int Id { get; set; }
    public int RoundNumber { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    
    public int GameSessionId { get; set; }
    public GameSession GameSession { get; set; } = null!;
    
    public List<PlayerAnswer> PlayerAnswers { get; set; } = new();
}