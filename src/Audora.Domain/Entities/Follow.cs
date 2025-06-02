using Audora.Domain.Common;
using Audora.Domain.Common.Enums;

namespace Audora.Domain.Entities;

public class Follow : Entity
{
    public Guid ListenerId { get; init; }
    public Guid EntityId { get; init; }
    public EntityType EntityType { get; init; }
    public DateTime FollowedAt { get; init; }

    public Follow(Guid listenerId, Guid entityId, EntityType followTarget)
    {
        ListenerId = listenerId;
        EntityId = entityId;
        EntityType = followTarget;
        FollowedAt = DateTime.UtcNow;
    }

    private Follow()
    {
    }
}