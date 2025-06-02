using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Contracts.Common;
using Audora.Contracts.Reactions.Responses;
using Audora.Contracts.Users.Responses;
using Audora.Domain.Common.Enums;
using Audora.Domain.Entities;

namespace Audora.Application.Reactions.Queries.ListEntityReactions;

public record ListEntityReactionsQuery(Guid EntityId, EntityType EntityType, Pagination Pagination)
    : IQuery<PagedResponse<UserReactionResponse>>;

public class ListEntityReactionsQueryHandler
    : IQueryHandler<ListEntityReactionsQuery, PagedResponse<UserReactionResponse>>
{
    private readonly IReactionRepository _reactionRepository;

    public ListEntityReactionsQueryHandler(IReactionRepository reactionRepository)
    {
        _reactionRepository = reactionRepository;
    }

    public async Task<Result<PagedResponse<UserReactionResponse>>> Handle(ListEntityReactionsQuery request,
        CancellationToken cancellationToken)
    {
        var reactions = await _reactionRepository.GetAllByEntityAsync(request.EntityId, request.EntityType);

        return CreateResponse(reactions, request.Pagination);
    }

    private static PagedResponse<UserReactionResponse> CreateResponse(IQueryable<Reaction> reactions, Pagination pagination)
    {
        return reactions.Paginate(pagination).Select(r => new UserReactionResponse
        {
            User = new UserResponse
            {
                Id = r.ListenerId,
                Name = "Dummy Name",
                AvatarUrl = "Dummy URL"
            },
            Reaction = r.ToResponse()
        }).ToPagedResponse(pagination, 0);
    }
}