namespace Audora.Contracts.PlaybackSessions.Requests;

public class MarkSessionProgressRequest
{
    public int PlaybackPosition { get; init; }
    public int ListenedDuration { get; init; }
}