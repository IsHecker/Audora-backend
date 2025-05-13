using Audora.Contracts.Reactions.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Mappings;

public static class ListenerReactionMapping
{
    public static ListenerReactionResponse ToResponse(this Reaction reaction)
    {
        return new ListenerReactionResponse
        {
            Reaction = reaction.ReactionType.ToString(), EntityType = reaction.EntityType
        };
    }
}