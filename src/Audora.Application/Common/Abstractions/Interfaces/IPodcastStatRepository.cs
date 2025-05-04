using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IPodcastStatRepository
{
    Task<PodcastStat> GetPodcastStatByIdAsync(Guid podcastStatId);
    Task<PodcastStat> GetPodcastStatByPodcastIdAsync(Guid podcastId);
    Task<bool> UpdatePodcastStatsAsync(PodcastStat podcastStat);
    IPodcastStatRepository IncludePodcast(bool includePodcast = true);
}