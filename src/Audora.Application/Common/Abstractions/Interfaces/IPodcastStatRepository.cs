using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IPodcastStatRepository
{
    Task<PodcastStat> GetByIdAsync(Guid podcastStatId);
    Task<PodcastStat> GetByPodcastIdAsync(Guid podcastId);
    Task UpdateAsync(PodcastStat podcastStat);
    
    IPodcastStatRepository IncludePodcast(bool includePodcast = true);
}