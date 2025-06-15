using Audora.Application.Auth.DTOs;
using Audora.Contracts.Auth.Requests;
using Audora.Contracts.Auth.Responses;

namespace Audora.Application.Common.Mappings;

public static class AuthMapping
{
    public static UserCredentialsDto ToUserCredentials(this AuthRequest request)
    {
        return new UserCredentialsDto
        {
            Email = request.Email,
            Password = request.Password,
            FullName = request.FirstName + request.LastName,
            Role = request.Role
        };
    }

    public static AuthResponse ToResponse(this AuthResult authResult)
    {
        return new AuthResponse
        {
            AccessToken = authResult.AccessToken,
            ExpiresIn = authResult.ExpiresIn,
            IdToken = authResult.IdToken,
            RefreshToken = authResult.RefreshToken,
            RequiresRoleSelection = authResult.RequiresRoleSelection
        };
    }
}