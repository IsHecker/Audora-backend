using Audora.Contracts.Comments.Requests;
using Audora.Contracts.Comments.Responses;
using Audora.Contracts.EngagementStats.Responses;
using Audora.Domain.Common.Enums;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Mappings;

public static class CommentMapping
{
    public static Comment ToDomain(this CreateCommentRequest request, Guid entityId, Guid listenerId, EntityType entityType)
    {
        return new Comment(listenerId, entityId, entityType, request.Content, request.ParentId);
    }

    public static CommentResponse ToResponse(this Comment comment,
        EngagementStatsResponse? engagementStat)
    {
        return new CommentResponse
        {
            Id = comment.Id,
            Content = comment.Content,
            EditedAt = comment.UpdatedAt,
            ParentId = comment.ParentId,
            PostedAt = comment.CreatedAt,
            ListenerId = comment.ListenerId,
            Engagements = engagementStat
        };
    }

    public static IEnumerable<CommentResponse> ToResponse(this IEnumerable<Comment> comments,
        Dictionary<Guid, EngagementStatsResponse> engagementStatsResponseDict,
        Dictionary<Guid, Reaction> listenerReactionsDict)
    {
        return comments.Select(comment =>
        {
            engagementStatsResponseDict.TryGetValue(comment.Id, out var engagementStat);

            if (engagementStat is not null)
            {
                listenerReactionsDict.TryGetValue(comment.Id, out var listenerReaction);
                engagementStat.ListenerReaction = listenerReaction?.ToResponse();
            }

            return comment.ToResponse(engagementStat);
        });
    }
}