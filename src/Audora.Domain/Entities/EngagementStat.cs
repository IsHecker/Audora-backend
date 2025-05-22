using Audora.Domain.Common;
using Audora.Domain.Common.Enums;

namespace Audora.Domain.Entities;

public class EngagementStat : Entity
{
    public Guid EntityId { get; init; }
    public EntityType EntityType { get; init; }
    public int Likes { get; private set; }
    public int Dislikes { get; private set; }
    public int Comments { get; private set; }


    public EngagementStat(Guid entityId, EntityType entityType)
    {
        EntityId = entityId;
        EntityType = entityType;
    }

    private EngagementStat()
    {
    }


    public void AddLike() => Likes++;

    public void RemoveLike()
    {
        if (Likes < 1)
            return;

        Likes--;
    }

    public void AddDislike() => Dislikes++;

    public void RemoveDislike()
    {
        if (Dislikes < 1)
            return;

        Dislikes--;
    }

    public void AddComment() => Comments++;
}