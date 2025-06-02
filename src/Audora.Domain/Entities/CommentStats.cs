using Audora.Domain.Common.Enums;

namespace Audora.Domain.Entities;

public class CommentStat
{
    public Guid EntityId { get; init; }
    public EntityType EntityType { get; init; }
    public int CommentCount { get; private set; } = 0;

    public void IncreaseCommentCount() => CommentCount++;
    public void DecreaseCommentCount() => CommentCount--;
}