namespace Audora.Contracts.PlaybackSessions.Responses;

public class CreateSessionResponse
{
    public int PlaybackPosition { get; init; }
    public TimeSpan ListenedDuration { get; init; }
    public DateTime StartedAt { get; init; }
    public DateTime LastPlayedAt { get; init; }
    public DateTime? FinishedAt { get; init; }
    public bool IsCompleted { get; init; }

    public bool IsSessionExpired => LastPlayedAt < DateTime.Today;
}