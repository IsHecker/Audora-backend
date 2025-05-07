using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure.Repositories;

public class PodcastStatRepository : Repository<PodcastStat>, IPodcastStatRepository
{
    public PodcastStatRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PodcastStat> GetByPodcastIdAsync(Guid podcastId)
    {
        return await Query.FirstAsync(p => p.PodcastId == podcastId);
    }

    public IPodcastStatRepository IncludePodcast(bool includePodcast = true)
    {
        if (includePodcast)
            Query = Query.Include(ps => ps.Podcast);

        return this;
    }
}