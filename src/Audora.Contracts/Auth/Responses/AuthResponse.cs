namespace Audora.Contracts.Auth.Responses;

public class AuthResponse
{
    public string? AccessToken { get; set; } = null!;
    public long? ExpiresIn { get; set; } = null!;
    public string? RefreshToken { get; set; } = null!; // Optional

    // true when signing in using google for the first time.
    public string? IdToken { get; set; } = null!;
    public bool? RequiresRoleSelection { get; init; } = null!;
}