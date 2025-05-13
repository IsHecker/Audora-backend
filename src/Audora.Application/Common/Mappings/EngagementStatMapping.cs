using Audora.Contracts.Reactions.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Mappings;

public static class EngagementStatMapping
{
    public static EngagementStatResponse ToResponse(this EngagementStat engagementStat, Reaction? listenerReaction)
    {
        return new EngagementStatResponse
        {
            Likes = engagementStat.Likes,
            Dislikes = engagementStat.Dislikes,
            Comments = engagementStat.Comments,
            ListenerReaction = listenerReaction?.ToResponse()
        };
    }
}