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
        return Task.FromResult(Query.Where(ep => ep.PodcastId == podcastId)
            .OrderBy(ep => ep.EpisodeNumber)
            .AsQueryable());
    }

    public async Task<IQueryable<Episode>> GetAllByPlaylistIdAsync(Guid playlistId)
    {
        return Context.PlaylistEpisodes
            .Where(pe => pe.PlaylistId == playlistId)
            .OrderBy(pe => pe.Order)
            .Join(Query, pe => pe.EpisodeId, e => e.Id, (pe, e) => e);
    }
}