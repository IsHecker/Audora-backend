using Audora.Domain.Common;
using Audora.Domain.Common.Enums;

namespace Audora.Domain.Entities;

public class Comment : Entity
{
    public Guid ListenerId { get; init; }
    public Guid EntityId { get; init; }
    public Guid? ParentId { get; init; }
    public EntityType EntityType { get; init; }
    public string Content { get; private set; } = null!;

    public Comment(
        Guid listenerId,
        Guid entityId,
        EntityType entityType,
        string content,
        Guid? parentId = null)
    {
        ListenerId = listenerId;
        ParentId = parentId;
        EntityId = entityId;
        EntityType = entityType;
        Content = content;
    }

    private Comment()
    {
    }

    public void ChangeContent(string newContent)
    {
        Content = newContent;
    }
}