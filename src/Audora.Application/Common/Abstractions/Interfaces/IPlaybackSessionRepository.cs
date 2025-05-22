using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IPlaybackSessionRepository : IRepository<PlaybackSession>
{
    Task<PlaybackSession?> GetAsync(Guid listenerId, Guid episodeId);
}