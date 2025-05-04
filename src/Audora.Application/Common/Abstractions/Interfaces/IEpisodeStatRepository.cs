using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IEpisodeStatRepository
{
    Task<EpisodeStat> GetEpisodeStatByEpisodeIdAsync(Guid episodeId);
    Task<IQueryable<EpisodeStat>> GetEpisodeStatsByPodcastStateId(Guid podcastStatId);
    Task<IQueryable<EpisodeStat>> GetStatsByEpisodeIdsAsync(IEnumerable<Guid> episodeIds);
}