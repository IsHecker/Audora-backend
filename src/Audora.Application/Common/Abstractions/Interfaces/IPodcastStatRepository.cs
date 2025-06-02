using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IPodcastStatRepository : IRepository<PodcastStat, IPodcastStatRepository>
{
    Task<PodcastStat?> GetByPodcastIdAsync(Guid podcastId);

    IPodcastStatRepository IncludePodcast(bool includePodcast = true);
}