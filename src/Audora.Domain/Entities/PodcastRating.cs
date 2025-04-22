using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class PodcastRating
{
    public Guid PodcastId { get; init; }
    public Guid ListenerId { get; init; }
    public byte Rating { get; init; }
    public DateTime AddedAt { get; init; }

    public PodcastRating(Guid podcastId, Guid listenerId, byte rating, DateTime addedAt)
    {
        PodcastId = podcastId;
        ListenerId = listenerId;
        Rating = rating;
        AddedAt = addedAt;
    }

    private PodcastRating()
    {
    }
}