using Audora.Contracts.Comments.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Mappings;

public static class CommentMapping
{
    public static CommentResponse ToResponse(this Comment comment,
        EngagementStat engagementStat,
        Reaction listenerReaction)
    {
        return new CommentResponse
        {
            Id = comment.Id,
            Content = comment.Content,
            EditedAt = comment.UpdatedAt,
            ParentId = comment.ParentId,
            PostedAt = comment.CreatedAt,
            ListenerId = comment.ListenerId,
            Engagements = engagementStat.ToResponse(listenerReaction)
        };
    }

    public static CommentsResponse ToResponse(this IQueryable<Comment> comments,
        Dictionary<Guid, EngagementStat> engagementStatsDict,
        Dictionary<Guid, Reaction> listenerReactionsDict)
    {
        return new CommentsResponse
        {
            Comments = comments.Select(comment =>
                comment.ToResponse(engagementStatsDict[comment.Id], listenerReactionsDict[comment.Id]))
        };
    }
}