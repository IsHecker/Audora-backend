using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IPodcastRepository
{
    Task<IQueryable<Podcast>> GetAllAsync();
    Task<Podcast?> GetByIdAsync(Guid id);
    Task AddAsync(Podcast podcast);
    Task<bool> DeleteAsync(Guid requestPodcastId);

    IPodcastRepository IncludeEpisodes(bool includeEpisodes = true);
}