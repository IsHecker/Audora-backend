using Audora.Domain.Common;
using Audora.Domain.Common.Enums;

namespace Audora.Domain.Entities;

public class Comment : Entity
{
    public Guid ListenerId { get; init; }
    public Guid ParentId { get; init; }
    public Guid EntityId { get; init; }
    public EntityType EntityType { get; init; }
    public string Content { get; private set; } = null!;

    public Comment(
        Guid listenerId,
        Guid parentId,
        Guid entityId,
        EntityType entityType,
        string content)
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
    
    public void EditContent(string newContent)
    {
        Content = newContent;
    }
}