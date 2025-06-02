using Audora.Domain.Common.Enums;

namespace Audora.Domain.Entities;

public class ReactionStat
{
    public Guid EntityId { get; init; }
    public EntityType EntityType { get; init; }
    public ReactionType ReactionType { get; init; }
    public int Count { get; private set; } = 0;

    public void IncreaseCount() => Count++;
    public void DecreaseCount() => Count--;
}
