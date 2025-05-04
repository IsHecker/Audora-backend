namespace Audora.Application.Common.Models;

public class User
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string? AvatarUrl { get; init; }
}