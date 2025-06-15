using Audora.Application.Common.Abstractions.Interfaces.Repositories;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Results;
using Audora.Contracts.Reactions.Responses;
using Audora.Domain.Common.Enums;

namespace Audora.Application.Reactions.Queries.GetListenerReaction;

public record GetListenerReactionQuery(Guid ListenerId, Guid EntityId, EntityType EntityType) : IQuery<ReactionResponse>;

public class GetListenerReactionQueryHandler
    : IQueryHandler<GetListenerReactionQuery, ReactionResponse>
{
    private readonly IReactionRepository _reactionRepository;

    public GetListenerReactionQueryHandler(IReactionRepository reactionRepository)
    {
        _reactionRepository = reactionRepository;
    }

    public async Task<Result<ReactionResponse>> Handle(GetListenerReactionQuery request,
        CancellationToken cancellationToken)
    {
        var reaction = await _reactionRepository.GetAsync(request.ListenerId, request.EntityId, request.EntityType);
        if (reaction is null)
        {
            return Error.NotFound(description: $"Reaction For ListenerId '{request.ListenerId}' is not found.");
        }

        return reaction.ToResponse();
    }
}