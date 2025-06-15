using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces.Repositories;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Application.Common.Services;
using Audora.Contracts.Comments.Responses;
using Audora.Contracts.Common;
using Audora.Domain.Common.Enums;
using Audora.Domain.Entities;

namespace Audora.Application.Comments.Queries.ListComments;

public record ListCommentsQuery(Guid? ListenerId, Guid? EntityId, Guid? ParentId, EntityType EntityType, Pagination Pagination) : IQuery<PagedResponse<CommentResponse>>;

public class ListCommentsQueryHandler : IQueryHandler<ListCommentsQuery, PagedResponse<CommentResponse>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IReactionRepository _reactionRepository;
    private readonly EngagementStatsService _engagementStatsService;

    public ListCommentsQueryHandler(
        ICommentRepository commentRepository,
        IReactionRepository reactionRepository,
        EngagementStatsService engagementStatsService)
    {
        _commentRepository = commentRepository;
        _reactionRepository = reactionRepository;
        _engagementStatsService = engagementStatsService;
    }

    public async Task<Result<PagedResponse<CommentResponse>>> Handle(ListCommentsQuery request,
          CancellationToken cancellationToken)
    {
        // TODO maybe separate comments retrieving from reactions retrieving into two different use cases.

        var comments = (await GetCommentsAsync(request.EntityType, request.EntityId, request.ParentId))
            .Paginate(request.Pagination);

        var commentIds = comments.Select(c => c.Id);

        // TODO maybe instead of dictionary, convert them directly to response objects.
        if (request.ListenerId is null)
        {
            return comments.ToResponse(null, null).ToPagedResponse(request.Pagination, -1);
        }

        var listenerReactionsDict = (await _reactionRepository.GetAllByEntityIdsAsync(commentIds))
                .Where(r => r.ListenerId == request.ListenerId)
                .ToDictionary(k => k.EntityId);

        var engagementStatResult = await _engagementStatsService.GetStatsAsync(commentIds, EntityType.Comment);

        if (engagementStatResult.IsError)
            return engagementStatResult.Errors;

        return comments.ToResponse(engagementStatResult.Value, listenerReactionsDict)
            .ToPagedResponse(request.Pagination, -1);
    }

    private async Task<IQueryable<Comment>> GetCommentsAsync(EntityType entityType, Guid? entityId = null, Guid? parentId = null)
    {
        if (entityId is not null)
            return await _commentRepository.GetByEntityAsync(entityId.Value, entityType);

        if (parentId is not null)
            return await _commentRepository.GetByParentCommentAsync(parentId.Value);

        throw new InvalidOperationException("All ids are null!");
    }
}