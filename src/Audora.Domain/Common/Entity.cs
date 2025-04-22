namespace Audora.Domain.Common;

public class Entity
{
    public Guid Id { get; private set; } = Guid.Empty;
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public DateTime? UpdatedAt { get; init; }
    

    public Entity(Guid id) => Id = id;

    protected Entity()
    {
    }
}