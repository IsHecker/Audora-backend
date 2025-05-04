using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mapping;
using Audora.Application.Common.Results;
using Audora.Contracts.Reactions.Responses;

namespace Audora.Application.Reactions.Queries.GetListenerEntityReaction;

public record GetListenerEntityReactionQuery(Guid ListenerId) : IQuery<ListenerEntityReactionResponse>;

public class GetListenerReactionForEntityQueryHandler
    : IQueryHandler<GetListenerEntityReactionQuery, ListenerEntityReactionResponse>
{
    private readonly IReactionRepository _reactionRepository;

    public GetListenerReactionForEntityQueryHandler(IReactionRepository reactionRepository)
    {
        _reactionRepository = reactionRepository;
    }

    public async Task<Result<ListenerEntityReactionResponse>> Handle(GetListenerEntityReactionQuery request,
        CancellationToken cancellationToken)
    {
        var reaction = await _reactionRepository.GetByListenerIdAsync(request.ListenerId);
        if (reaction is null)
        {
            return Error.NotFound(description: $"Reaction For ListenerId '{request.ListenerId}' is not found.");
        }

        return reaction.ToResponse();
    }
}