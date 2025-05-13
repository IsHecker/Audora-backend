using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IEpisodeRepository
{
    Task<IQueryable<Episode>> GetAllAsync();
    Task<IQueryable<Episode>> GetAllByPodcastIdAsync(Guid podcastId);
    Task<IQueryable<Episode>> GetAllByPlaylistIdAsync(Guid playlistId);
    Task<Episode?> GetByIdAsync(Guid id);
    Task UpdateAsync(Episode episode);
    Task<bool> DeleteAsync(Guid id);
}