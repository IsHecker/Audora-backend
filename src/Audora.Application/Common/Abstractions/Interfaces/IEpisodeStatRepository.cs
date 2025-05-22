using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IEpisodeStatRepository : IRepository<EpisodeStat>
{
    Task<EpisodeStat> GetByEpisodeIdAsync(Guid episodeId);
    Task<IQueryable<EpisodeStat>> GetAllByPodcastId(Guid podcastId);
    Task<IQueryable<EpisodeStat>> GetAllByEpisodeIdsAsync(IEnumerable<Guid> episodeIds);
}