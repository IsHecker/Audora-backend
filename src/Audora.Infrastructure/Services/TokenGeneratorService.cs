using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Audora.Application.Auth.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Audora.Infrastructure.Services;

public class TokenGeneratorService
{
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(8);

    private readonly IConfiguration _configuration;

    public TokenGeneratorService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AuthResult GenerateToken(UserCredentialsDto userCreds, Guid userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!);
        var expiresIn = DateTimeOffset.UtcNow.Add(TokenLifetime);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()), // your internal user ID
            new(JwtRegisteredClaimNames.Email, userCreds.Email),
            new("name", userCreds.FullName!),
            new(ClaimTypes.Role, userCreds.Role!),
            new("auth_provider", "Google"),
            new("picture", userCreds.ProfilePictureUrl ?? ""),
            new(JwtRegisteredClaimNames.Iat,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresIn.DateTime,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthResult
        {
            AccessToken = tokenHandler.WriteToken(token),
            ExpiresIn = expiresIn.ToUnixTimeMilliseconds(),
            RefreshToken = ""
        };
    }
}