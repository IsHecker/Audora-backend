using Audora.Domain.Common;
using Audora.Domain.Common.Enums;

namespace Audora.Domain.Entities;

public class Reaction : Entity
{
    public Guid EntityId { get; init; }
    public Guid ListenerId { get; init; }
    public EntityType EntityType { get; init; }
    public ReactionType ReactionType { get; private set; }

    public Reaction(
        Guid listenerId,
        Guid entityId,
        EntityType entityType,
        ReactionType reactionType)
    {
        ListenerId = listenerId;
        EntityId = entityId;
        EntityType = entityType;
        ReactionType = reactionType;
    }

    private Reaction()
    {
    }

    public void UpdateReactionType(ReactionType reactionType) => ReactionType = reactionType;
}

public enum ReactionType : byte
{
    Like,
    Dislike
}