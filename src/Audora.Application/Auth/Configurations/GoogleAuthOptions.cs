namespace Audora.Application.Auth.Configurations;

public class GoogleAuthOptions
{
    public const string SectionName = "Google";

    public string Issuer { get; init; } = null!;
    public string SignInUrl { get; init; } = null!;
    public string TokenUrl { get; init; } = null!;
    public string CertsUrl { get; init; } = null!;
    public string ClientId { get; init; } = null!;
    public string ClientSecret { get; init; } = null!;
    public string RedirectUri { get; init; } = null!;
    public string Scope { get; init; } = null!;
}
