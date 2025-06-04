namespace Audora.Contracts.PlaybackSessions.Responses;

public class PlaybackSessionResponse
{
    public Guid Id { get; init; }
    public Guid EpisodeId { get; init; }
    public int PlaybackPosition { get; init; }
    public int TotalListenedDuration { get; init; }
    public DateTime StartedAt { get; init; }
    public DateTime LastPlayedAt { get; init; }
    public DateTime? FinishedAt { get; init; }
    public bool IsCompleted { get; init; }

    public bool IsSessionExpired => LastPlayedAt.Date < DateTime.UtcNow.Date;
}