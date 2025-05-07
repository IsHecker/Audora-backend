using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class Follow : Entity
{
    public Guid FollowerId { get; init; }
    public Guid EntityId { get; init; }
    public FollowTarget FollowTarget { get; init; }
    public DateTime FollowedAt { get; init; }

    public Follow(Guid followerId, Guid entityId, FollowTarget followTarget)
    {
        FollowerId = followerId;
        EntityId = entityId;
        FollowTarget = followTarget;
        FollowedAt = DateTime.UtcNow;
    }

    private Follow()
    {
    }
}

public enum FollowTarget : byte
{
    Creator,
    Podcast
}