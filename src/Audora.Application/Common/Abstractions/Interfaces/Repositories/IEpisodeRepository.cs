using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces.Repositories;

public interface IEpisodeRepository : IRepository<Episode, IEpisodeRepository>
{
    Task<IQueryable<Episode>> GetAllByPodcastIdAsync(Guid podcastId);
    Task<IQueryable<Episode>> GetAllByPlaylistIdAsync(Guid playlistId);
}