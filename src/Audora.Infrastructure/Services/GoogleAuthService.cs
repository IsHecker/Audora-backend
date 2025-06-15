using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using Audora.Application.Auth.DTOs;
using Audora.Application.Common.Abstractions.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;

namespace Audora.Infrastructure.Services;

public class GoogleAuthService : IGoogleAuthService
{
    private readonly HttpClient _httpClient;

    public GoogleAuthService(HttpClient client)
    {
        _httpClient = client;
    }

    public async Task<GoogleTokenDto> ExchangeCodeAsync(
        string tokenUrl,
        string code,
        string redirectUri,
        string clientId,
        string clientSecret)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl)
        {
            Content = new FormUrlEncodedContent([
                new KeyValuePair<string, string>("code",code),
                new KeyValuePair<string, string>("client_id",clientId),
                new KeyValuePair<string, string>("client_secret",clientSecret),
                new KeyValuePair<string, string>("redirect_uri",redirectUri),
                new KeyValuePair<string, string>("grant_type","authorization_code"),
            ])
        };

        var response = await _httpClient.SendAsync(request);

        var token = await response.Content.ReadFromJsonAsync<GoogleTokenDto>();
        return token ?? throw new Exception("Failed to parse token response.");
    }

    public async Task<UserCredentialsDto> ValidateAndExtractIdTokenAsync(string certsUrl, string idToken, string iss, string aud)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(idToken);
        var kid = jwt.Header.Kid;

        var response = await _httpClient.GetFromJsonAsync<JsonElement>(certsUrl);
        var keys = response.GetProperty("keys");

        JsonElement matchedKey = default;
        foreach (var key in keys.EnumerateArray())
        {
            if (key.GetProperty("kid").GetString() == kid)
            {
                matchedKey = key;
                break;
            }
        }

        if (matchedKey.ValueKind == JsonValueKind.Undefined)
            throw new SecurityTokenException("Matching key not found.");

        var n = matchedKey.GetProperty("n").GetString()!;
        var e = matchedKey.GetProperty("e").GetString()!;

        var rsaParams = new RSAParameters
        {
            Modulus = Base64UrlDecode(n),
            Exponent = Base64UrlDecode(e)
        };

        using var rsa = RSA.Create();
        rsa.ImportParameters(rsaParams);

        var validationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = iss,
            ValidAudience = aud,
            IssuerSigningKey = new RsaSecurityKey(rsa),
        };
        var validationResults = await handler.ValidateTokenAsync(idToken, validationParameters);

        handler.ValidateToken(idToken, validationParameters, out var validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;

        return new UserCredentialsDto
        {
            ProviderUserId = jwtToken.Payload["sub"]?.ToString() ?? "",
            Provider = "Google",
            Email = jwtToken.Payload["email"]?.ToString() ?? "",
            FullName = jwtToken.Payload["name"]?.ToString() ?? "",
            ProfilePictureUrl = jwtToken.Payload["picture"]?.ToString() ?? "",
            GivenName = jwtToken.Payload["given_name"]?.ToString() ?? "",
            FamilyName = jwtToken.Payload["family_name"]?.ToString() ?? ""
        };
    }

    private static byte[] Base64UrlDecode(string input)
    {
        input = input.Replace('-', '+').Replace('_', '/');
        switch (input.Length % 4)
        {
            case 2: input += "=="; break;
            case 3: input += "="; break;
        }
        return Convert.FromBase64String(input);
    }
}



public class JwksResponse
{
    public List<Jwk> Keys { get; set; } = [];
}

public class Jwk
{
    public string Kty { get; set; } = null!; // Key Type (e.g., "RSA")
    public string Alg { get; set; } = null!; // Algorithm (e.g., "RS256")
    public string Use { get; set; } = null!; // Public key use (e.g., "sig")
    public string Kid { get; set; } = null!; // Key ID
    public string N { get; set; } = null!;   // Modulus (Base64url)
    public string E { get; set; } = null!;   // Exponent (Base64url)
}