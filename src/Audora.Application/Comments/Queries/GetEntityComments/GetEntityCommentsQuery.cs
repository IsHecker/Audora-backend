using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mapping;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Contracts.Comments.Responses;

namespace Audora.Application.Comments.Queries.GetEntityComments;

public record GetEntityCommentsQuery(Guid ListenerId, Guid EntityId, Pagination Pagination) : IQuery<CommentsResponse>;

public class GetEntityCommentsQueryHandler : IQueryHandler<GetEntityCommentsQuery, CommentsResponse>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IReactionRepository _reactionRepository;
    private readonly IEngagementStatRepository _engagementStatRepository;

    public GetEntityCommentsQueryHandler(ICommentRepository commentRepository, IReactionRepository reactionRepository,
        IEngagementStatRepository engagementStatRepository)
    {
        _commentRepository = commentRepository;
        _reactionRepository = reactionRepository;
        _engagementStatRepository = engagementStatRepository;
    }

    public async Task<Result<CommentsResponse>> Handle(GetEntityCommentsQuery request,
        CancellationToken cancellationToken)
    {
        var comments = (await _commentRepository.GetCommentsByEntityId(request.EntityId))
            .Paginate(request.Pagination);

        var commentIds = comments.Select(c => c.Id);

        var listenerReactionsDict = (await _reactionRepository
                .GetReactionsByEntityIdsAsync(commentIds))
            .Where(r => r.ListenerId == request.ListenerId)
            .ToDictionary(k => k.EntityId, v => v);

        var engagementStatDict = (await _engagementStatRepository.GetByEntityIdsAsync(commentIds))
            .ToDictionary(k => k.EntityId, v => v);

        return comments.ToResponse(engagementStatDict, listenerReactionsDict);
    }
}