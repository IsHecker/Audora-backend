using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure.Repositories;

public class PodcastRepository : Repository<Podcast>, IPodcastRepository
{
    public PodcastRepository(ApplicationDbContext context) : base(context)
    {
    }

    public IPodcastRepository IncludeEpisodes(bool includeEpisodes = true)
    {
        if (includeEpisodes)
        {
            Query = Query.Include(p => p.Episodes);
        }

        return this;
    }
}