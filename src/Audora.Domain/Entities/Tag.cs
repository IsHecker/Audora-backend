namespace Audora.Domain.Entities;

public class Tag
{
    public string Name { get; init; } = null!;

    public Tag(string name) => Name = name;

    private Tag()
    {
    }
}