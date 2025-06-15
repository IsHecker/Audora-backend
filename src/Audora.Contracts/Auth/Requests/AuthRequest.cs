namespace Audora.Contracts.Auth.Requests;

public class AuthRequest
{
    public string? FirstName { get; init; } = null!;
    public string? LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string? Role { get; init; } = null!;
}