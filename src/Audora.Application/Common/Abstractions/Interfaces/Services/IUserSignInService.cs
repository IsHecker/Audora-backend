using Audora.Application.Auth.DTOs;
using Audora.Application.Common.Results;

namespace Audora.Application.Common.Abstractions.Interfaces.Services;

public interface IUserSignInService
{
    Task<Result<AuthResult>> RegisterAsync(UserCredentialsDto credentials);
    Task<Result<AuthResult>> SignInAsync(UserCredentialsDto credentials);
    Task<bool> IsEmailExistingAsync(string email);
}