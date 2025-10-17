namespace MiniGameHub.Api.Models;

public class PlayerAnswer
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public Player Player { get; set; } = null!;
    public int RoundId { get; set; }
    public Round Round { get; set; } = null!;
    public string Answer { get; set; } = String.Empty;
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}