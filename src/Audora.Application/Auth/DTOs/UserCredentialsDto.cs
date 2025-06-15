namespace Audora.Application.Auth.DTOs;

public class UserCredentialsDto
{
    public string? ProviderUserId { get; set; } // e.g., Google sub, Null for signup form
    public string? FullName { get; set; } // from provider or signup form
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public string Email { get; set; } = null!;
    public string? Role { get; set; } = null!;
    public string? Provider { get; set; } // "Google", "Facebook", or null for local
    public string? ProfilePictureUrl { get; set; } // from provider
    public string? Password { get; set; } // Null for external providers
    public bool? EmailConfirmed { get; set; }
}