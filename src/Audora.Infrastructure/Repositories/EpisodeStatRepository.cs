using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure.Repositories;

public class EpisodeStatRepository : Repository<EpisodeStat>, IEpisodeStatRepository
{
    public EpisodeStatRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<EpisodeStat> GetByEpisodeIdAsync(Guid episodeId)
    {
        return await Query.FirstAsync(es => es.EpisodeId == episodeId);
    }

    public Task<IQueryable<EpisodeStat>> GetAllByPodcastId(Guid podcastId)
    {
        return Task.FromResult(Query.Where(es => es.PodcastId == podcastId));
    }

    public Task<IQueryable<EpisodeStat>> GetAllByEpisodeIdsAsync(IEnumerable<Guid> episodeIds)
    {
        return Task.FromResult(Query.Where(es => episodeIds.Contains(es.EpisodeId)));
    }
}