using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IPodcastRepository : IRepository<Podcast, IPodcastRepository>
{
    Task<IQueryable<Podcast>> GetCreatorPodcasts(Guid creatorId);
    IPodcastRepository IncludeEpisodes(bool includeEpisodes = true);

    IPodcastRepository WithPublishedPodcasts();
}