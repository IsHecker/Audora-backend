using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class PodcastRating : Entity
{
    public Guid PodcastId { get; init; }
    public Guid ListenerId { get; init; }
    public byte Rating { get; private set; }
    public DateTime AddedAt { get; init; }

    public PodcastRating(Guid podcastId, Guid listenerId, byte rating)
    {
        PodcastId = podcastId;
        ListenerId = listenerId;
        Rating = rating;
        AddedAt = DateTime.UtcNow;
    }

    private PodcastRating()
    {
    }

    public void SetRating(byte rating) => Rating = rating;
}