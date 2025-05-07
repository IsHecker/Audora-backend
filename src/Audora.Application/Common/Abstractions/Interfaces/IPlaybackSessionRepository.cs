using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IPlaybackSessionRepository
{
    Task<PlaybackSession?> GetAsync(Guid listenerId, Guid episodeId);
    Task AddAsync(PlaybackSession playbackSession);
}