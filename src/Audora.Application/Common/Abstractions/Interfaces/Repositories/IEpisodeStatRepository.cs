using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces.Repositories;

public interface IEpisodeStatRepository : IRepository<EpisodeStat, IEpisodeStatRepository>
{
    Task<EpisodeStat> GetByEpisodeIdAsync(Guid episodeId);
    Task<IQueryable<EpisodeStat>> GetAllByPodcastId(Guid podcastId);
    Task<IQueryable<EpisodeStat>> GetAllByEpisodeIdsAsync(IEnumerable<Guid> episodeIds);
}