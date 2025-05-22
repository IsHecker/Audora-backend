using Audora.Application.Search;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IPodcastRepository : IRepository<Podcast>
{
    IPodcastRepository IncludeEpisodes(bool includeEpisodes = true);

    IPodcastRepository WithPublishedPodcasts();
}