using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure.Repositories;

public class PodcastRatingRepository : Repository<PodcastRating>, IPodcastRatingRepository
{
    public PodcastRatingRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PodcastRating?> GetByEntityIdAsync(Guid podcastId)
    {
        return await Query.FirstOrDefaultAsync(pr => pr.PodcastId == podcastId);
    }

    public Task<IQueryable<PodcastRating>> GetAllByListenerIdAsync(Guid listenerId)
    {
        return Task.FromResult(Query.Where(pr => pr.ListenerId == listenerId));
    }
}