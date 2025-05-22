using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IEpisodeRepository : IRepository<Episode>
{
    Task<IQueryable<Episode>> GetAllByPodcastIdAsync(Guid podcastId);
    Task<IQueryable<Episode>> GetAllByPlaylistIdAsync(Guid playlistId);
}