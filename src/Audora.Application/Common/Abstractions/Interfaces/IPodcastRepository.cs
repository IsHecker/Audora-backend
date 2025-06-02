using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IPodcastRepository : IRepository<Podcast, IPodcastRepository>
{
    IPodcastRepository IncludeEpisodes(bool includeEpisodes = true);

    IPodcastRepository WithPublishedPodcasts();
}