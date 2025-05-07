using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Domain.Entities;

namespace Audora.Infrastructure.Repositories;

public class EpisodeRepository : Repository<Episode>, IEpisodeRepository
{
    public EpisodeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<IQueryable<Episode>> GetAllByPodcastIdAsync(Guid podcastId)
    {
        return Task.FromResult(Query.Where(ep => ep.PodcastId == podcastId));
    }
}