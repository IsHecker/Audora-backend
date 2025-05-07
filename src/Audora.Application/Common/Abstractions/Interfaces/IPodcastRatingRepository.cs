using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IPodcastRatingRepository
{
    Task<PodcastRating?> GetByEntityIdAsync(Guid podcastId);
    Task<IQueryable<PodcastRating>> GetAllByListenerIdAsync(Guid listenerId);
    Task AddAsync(PodcastRating podcastRating);
}