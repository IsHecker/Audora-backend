using Audora.Application.Auth.DTOs;

namespace Audora.Application.Common.Abstractions.Interfaces.Services;

public interface IGoogleAuthService
{
    Task<GoogleTokenDto> ExchangeCodeAsync(string tokenUrl, string code, string redirectUri, string clientId, string clientSecret);
    Task<UserCredentialsDto> ValidateAndExtractIdTokenAsync(string certsUrl, string idToken, string iss, string aud);
}