using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IEpisodeStatRepository
{
    Task<EpisodeStat> GetByEpisodeIdAsync(Guid episodeId);
    Task<IQueryable<EpisodeStat>> GetAllByPodcastStateId(Guid podcastStatId);
    Task<IQueryable<EpisodeStat>> GetAllByEpisodeIdsAsync(IEnumerable<Guid> episodeIds);
}