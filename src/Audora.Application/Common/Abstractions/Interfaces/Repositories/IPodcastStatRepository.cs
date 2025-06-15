using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces.Repositories;

public interface IPodcastStatRepository : IRepository<PodcastStat, IPodcastStatRepository>
{
    Task<PodcastStat?> GetByPodcastIdAsync(Guid podcastId);

    IPodcastStatRepository IncludePodcast(bool includePodcast = true);
}