using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Results;
using Audora.Contracts.Reactions.Responses;
using Audora.Domain.Common.Enums;

namespace Audora.Application.Reactions.Queries.GetListenerReaction;

public record GetListenerReactionQuery(Guid ListenerId, Guid EntityId, EntityType EntityType) : IQuery<ListenerReactionResponse>;

public class GetListenerReactionQueryHandler
    : IQueryHandler<GetListenerReactionQuery, ListenerReactionResponse>
{
    private readonly IReactionRepository _reactionRepository;

    public GetListenerReactionQueryHandler(IReactionRepository reactionRepository)
    {
        _reactionRepository = reactionRepository;
    }

    public async Task<Result<ListenerReactionResponse>> Handle(GetListenerReactionQuery request,
        CancellationToken cancellationToken)
    {
        var reaction = await _reactionRepository.GetAsync(request.ListenerId, request.EntityId);
        if (reaction is null)
        {
            return Error.NotFound(description: $"Reaction For ListenerId '{request.ListenerId}' is not found.");
        }

        return reaction.ToResponse();
    }
}