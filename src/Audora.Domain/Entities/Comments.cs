using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class Comments : Entity
{
    public Guid ListenerId { get; init; }
    public Guid ParentId { get; init; }
    public Guid EntityId { get; init; }
    public string EntityType { get; init; } = null!;
    public string Content { get; init; } = null!;

    public Comments(
        Guid listenerId,
        Guid parentId,
        Guid entityId,
        string entityType,
        string content)
    {
        ListenerId = listenerId;
        ParentId = parentId;
        EntityId = entityId;
        EntityType = entityType;
        Content = content;
    }

    private Comments()
    {
    }
}