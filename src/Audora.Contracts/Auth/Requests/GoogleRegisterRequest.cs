namespace Audora.Contracts.Auth.Requests;

public class GoogleRegisterRequest
{
    public string IdToken { get; init; } = null!;
    public string Role { get; init; } = null!;
}