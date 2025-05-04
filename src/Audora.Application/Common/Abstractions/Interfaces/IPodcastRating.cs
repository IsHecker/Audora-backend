using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IPodcastRating
{
    Task<PodcastRating?> GetListenerRatingByPodcastIdAsync(Guid podcastId);
    Task AddAsync(Guid podcastId, Guid listenerId, byte rating);
}