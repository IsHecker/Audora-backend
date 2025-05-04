using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class PlaybackSession : Entity
{
    public Guid ListenerId { get; init; }
    public Guid EpisodeId { get; init; }
    public int PlaybackPosition { get; private set; }
    public TimeSpan ListenedDuration { get; private set; }
    public DateTime StartedAt { get; init; }
    public DateTime LastPlayedAt { get; private set; }
    public DateTime? FinishedAt { get; private set; }
    public bool IsCompleted { get; private set; }

    public bool IsSessionExpired => LastPlayedAt < DateTime.Today;
    public bool IsFirstTimeListening => LastPlayedAt > StartedAt;

    public PlaybackSession(Guid listenerId, Guid episodeId)
    {
        ListenerId = listenerId;
        EpisodeId = episodeId;
        StartedAt = DateTime.UtcNow;
        LastPlayedAt = StartedAt;
    }

    private PlaybackSession()
    {
    }

    public void MarkProgress(int playbackPosition, TimeSpan listenedDuration, bool isCompleted)
    {
        // TODO Result pattern Errors instead of exceptions.
        if (playbackPosition < 0)
            throw new ArgumentOutOfRangeException(nameof(playbackPosition));

        if (listenedDuration < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(listenedDuration));

        if (listenedDuration < ListenedDuration)
            throw new InvalidOperationException("Listened duration cannot decrease.");

        PlaybackPosition = playbackPosition;
        ListenedDuration = listenedDuration;

        if (!isCompleted || IsCompleted)
            return;

        IsCompleted = true;
        FinishedAt = DateTime.UtcNow;
    }

    public void UpdateLastPlayedAt()
    {
        LastPlayedAt = DateTime.UtcNow;
    }
}