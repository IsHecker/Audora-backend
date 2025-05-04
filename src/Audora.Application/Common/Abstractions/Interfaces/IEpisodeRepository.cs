using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IEpisodeRepository
{
    Task<IQueryable<Episode>> GetEpisodesAsync();
    Task<IQueryable<Episode>> GetEpisodesByPodcastIdAsync(Guid podcastId);
    Task<Episode?> GetByIdAsync(Guid id);
    Task UpdateAsync(Episode episode);
    Task<bool> DeleteAsync(Guid id);
}