using Audora.Contracts.PlaybackSessions.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Mappings;

public static class PlaybackSessionMapping
{
    public static PlaybackSessionResponse ToResponse(this PlaybackSession session)
    {
        return new PlaybackSessionResponse
        {
            Id = session.Id,
            EpisodeId = session.EpisodeId,
            PlaybackPosition = session.PlaybackPosition,
            FinishedAt = session.FinishedAt,
            StartedAt = session.StartedAt,
            IsCompleted = session.IsCompleted,
            LastPlayedAt = session.LastPlayedAt,
            TotalListenedDuration = session.TotalListenedDuration
        };
    }

    public static IEnumerable<PlaybackSessionResponse> ToResponse(this IEnumerable<PlaybackSession> sessions)
    {
        return sessions.Select(ToResponse);
    }
}