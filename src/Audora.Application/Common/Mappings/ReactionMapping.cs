using Audora.Contracts.Reactions.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Mappings;

public static class ListenerReactionMapping
{
    public static ReactionResponse ToResponse(this Reaction reaction)
    {
        return new ReactionResponse
        {
            Reaction = reaction.ReactionType.ToString(),
            EntityType = reaction.EntityType.ToString()
        };
    }
}