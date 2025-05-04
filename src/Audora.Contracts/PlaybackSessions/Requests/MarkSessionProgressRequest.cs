namespace Audora.Contracts.PlaybackSessions.Requests;

public class MarkSessionProgressRequest
{
    public int PlaybackPosition { get; init; }
    public TimeSpan ListenedDuration { get; init; }
    public bool IsCompleted { get; init; }
}