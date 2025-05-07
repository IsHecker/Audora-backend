using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Contracts.Comments.Responses;

namespace Audora.Application.Comments.Queries.ListEntityComments;

public record ListEntityCommentsQuery(Guid ListenerId, Guid EntityId, Pagination Pagination) : IQuery<CommentsResponse>;

public class ListEntityCommentsQueryHandler : IQueryHandler<ListEntityCommentsQuery, CommentsResponse>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IReactionRepository _reactionRepository;
    private readonly IEngagementStatRepository _engagementStatRepository;

    public ListEntityCommentsQueryHandler(ICommentRepository commentRepository, IReactionRepository reactionRepository,
        IEngagementStatRepository engagementStatRepository)
    {
        _commentRepository = commentRepository;
        _reactionRepository = reactionRepository;
        _engagementStatRepository = engagementStatRepository;
    }

    public async Task<Result<CommentsResponse>> Handle(ListEntityCommentsQuery request,
        CancellationToken cancellationToken)
    {
        // TODO maybe separate comments retrieving from reactions retrieving into two different use cases.
        
        var comments = (await _commentRepository.GetAllByEntityId(request.EntityId))
            .Paginate(request.Pagination);

        var commentIds = comments.Select(c => c.Id);

        // TODO maybe instead of dictionary, convert them directly to response objects.
        var listenerReactionsDict = (await _reactionRepository.GetAllByEntityIdsAsync(commentIds))
            .Where(r => r.ListenerId == request.ListenerId)
            .ToDictionary(k => k.EntityId, v => v);

        var engagementStatDict = (await _engagementStatRepository.GetByEntityIdsAsync(commentIds))
            .ToDictionary(k => k.EntityId, v => v);

        return comments.ToResponse(engagementStatDict, listenerReactionsDict);
    }
}