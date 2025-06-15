using Audora.Application.Common.Abstractions.Interfaces.Repositories;
using Audora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure.Repositories;

public class PodcastRepository : Repository<Podcast, IPodcastRepository>, IPodcastRepository
{
    public PodcastRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<IQueryable<Podcast>> GetCreatorPodcasts(Guid creatorId)
    {
        return Task.FromResult(Query.Where(podcast => podcast.CreatorId == creatorId));
    }

    public IPodcastRepository IncludeEpisodes(bool includeEpisodes = true)
    {
        if (includeEpisodes)
        {
            Query = Query.Include(p => p.Episodes);
        }

        return this;
    }

    public IPodcastRepository WithPublishedPodcasts()
    {
        Query = Query.Where(podcast => podcast.IsPublished);
        return this;
    }
}