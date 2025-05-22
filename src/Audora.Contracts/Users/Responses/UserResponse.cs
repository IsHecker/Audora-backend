namespace Audora.Contracts.Users.Responses;

public class UserResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string? AvatarUrl { get; init; }
}