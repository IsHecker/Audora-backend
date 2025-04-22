using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class PlaybackHistory
{
    public Guid ListenerId { get; init; }
    public Guid EpisodeId { get; init; }
    public DateTime PlayedAt { get; init; }

    public PlaybackHistory(
        Guid listenerId,
        Guid episodeId,
        DateTime playedAt)
    {
        ListenerId = listenerId;
        EpisodeId = episodeId;
        PlayedAt = playedAt;
    }

    private PlaybackHistory()
    {
    }
}