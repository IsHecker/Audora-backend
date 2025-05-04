using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IPlaybackSessionRepository
{
    Task<PlaybackSession> AddAsync(Guid listenerId, Guid episodeId);
    Task<PlaybackSession?> GetAsync(Guid listenerId, Guid episodeId);
    Task UpdateAsync(PlaybackSession  playbackSession);
}