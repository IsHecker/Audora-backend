using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces.Repositories;

public interface IPlaybackSessionRepository : IRepository<PlaybackSession, IPlaybackSessionRepository>
{
    Task<PlaybackSession?> GetAsync(Guid listenerId, Guid episodeId);
    Task<IQueryable<PlaybackSession>> GetAllByListenerId(Guid listenerId);
}