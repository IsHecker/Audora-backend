using Audora.Contracts.EngagementStats.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Mappings;

public static class EngagementStatMapping
{
    public static EngagementStatsResponse ToResponse(
        IEnumerable<ReactionStat>? reactionStats,
        int commentCount,
        Reaction? listenerReaction = null)
    {
        return new EngagementStatsResponse
        {
            CommentCount = commentCount,
            Reactions = reactionStats?.ToDictionary(r => r.ReactionType.ToString(), r => r.Count),

            ListenerReaction = listenerReaction?.ToResponse()
        };
    }
}