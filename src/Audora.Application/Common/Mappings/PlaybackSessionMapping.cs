using Audora.Contracts.PlaybackSessions.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Mappings;

public static class PlaybackSessionMapping
{
    public static CreateSessionResponse ToResponse(this PlaybackSession session)
    {
        return new CreateSessionResponse
        {
            PlaybackPosition = session.PlaybackPosition,
            FinishedAt = session.FinishedAt,
            StartedAt = session.StartedAt,
            IsCompleted = session.IsCompleted,
            LastPlayedAt = session.LastPlayedAt,
            ListenedDuration = session.ListenedDuration
        };
    }
}