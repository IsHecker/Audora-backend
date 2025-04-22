using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class Reaction : Entity
{
    public Guid ListenerId { get; init; }
    public Guid EntityId { get; init; }
    public string EntityType { get; init; } = null!;
    public ReactionType ReactionType { get; init; }

    public Reaction(
        Guid listenerId,
        Guid entityId,
        string entityType,
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
}

public enum ReactionType : byte
{
    Like,
    Dislike
}