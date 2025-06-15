using Audora.Application.Common.Results;

namespace Audora.Application.Auth.DTOs;

public class AuthResult
{
    public string? AccessToken { get; set; } = null!;
    public long? ExpiresIn { get; set; } = null!;
    public string? RefreshToken { get; set; } = null!; // Optional

    public string? IdToken { get; set; } = null!;
    // true when signing in using google for the first time.
    public bool? RequiresRoleSelection { get; init; } = null!;

    public List<Error>? Errors { get; init; } = null!;
}