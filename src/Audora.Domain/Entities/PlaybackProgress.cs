using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class PlaybackProgress : Entity
{
    public Guid ListenerId { get; init; }
    public Guid EpisodeId { get; init; }
    public long PlaybackPosition { get; init; }

    public PlaybackProgress(
        Guid listenerId,
        Guid episodeId,
        long playbackPosition)
    {
        ListenerId = listenerId;
        EpisodeId = episodeId;
        PlaybackPosition = playbackPosition;
    }

    private PlaybackProgress()
    {
    }
}