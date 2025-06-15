namespace Audora.Application.Auth.DTOs;

public class GoogleUserInfoDto
{
    public string Iss { get; init; } = null!;
    public string Aud { get; init; } = null!;
    public string Sub { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Picture { get; set; } = null!;
    public string GivenName { get; init; } = null!;
    public string FamilyName { get; init; } = null!;
}