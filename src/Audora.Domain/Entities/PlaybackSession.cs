using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class PlaybackSession : Entity
{
    private const float CompletionThreshold = 0.95f; // 95% listened to mark as completed

    public Guid ListenerId { get; init; }
    public Guid EpisodeId { get; init; }
    public int PlaybackPosition { get; private set; } // in seconds
    public int TotalListenedDuration { get; private set; } // in seconds
    public DateTime StartedAt { get; init; } // The date and time when the user first started playing the episode.
    public DateTime LastPlayedAt { get; private set; } // The most recent time the user played the episode.
    public DateTime? FinishedAt { get; private set; } // When the listener finished the episode (typically reaching the end).
    public bool IsCompleted { get; private set; }

    public bool IsSessionExpired => LastPlayedAt.Date < DateTime.UtcNow.Date;

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

    public void MarkProgress(int playbackPosition, int listenedDuration, long episodeDuration)
    {
        // TODO Result pattern Errors instead of exceptions.
        if (playbackPosition < 0 || playbackPosition > MathF.Floor(episodeDuration / 1000f))
            throw new ArgumentOutOfRangeException(nameof(playbackPosition));

        if (listenedDuration < 0)
            throw new ArgumentOutOfRangeException(nameof(listenedDuration));

        if (listenedDuration < 0)
            throw new InvalidOperationException("Listened duration cannot be less than 0.");

        PlaybackPosition = playbackPosition;
        TotalListenedDuration += listenedDuration;

        if (!IsCompleted && HasReachedCompletionThreshold(episodeDuration))
        {
            IsCompleted = true;
            FinishedAt = DateTime.UtcNow;
        }
    }

    public void UpdateLastPlayedAt()
    {
        LastPlayedAt = DateTime.UtcNow;
    }

    private bool HasReachedCompletionThreshold(long episodeDurationMs) =>
        PlaybackPosition >= MathF.Round(episodeDurationMs / 1000f * CompletionThreshold);
}