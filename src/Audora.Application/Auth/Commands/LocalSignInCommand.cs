using Audora.Application.Auth.DTOs;
using Audora.Application.Common.Abstractions.Interfaces.Services;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Results;
using Audora.Contracts.Auth.Responses;

namespace Audora.Application.Auth.Commands;

public record LocalSignInCommand(UserCredentialsDto Creds) : ICommand<AuthResponse>;

public class LocalSignInCommandHandler : ICommandHandler<LocalSignInCommand, AuthResponse>
{
    private readonly IUserSignInService _userSignInService;

    public LocalSignInCommandHandler(
        IUserSignInService userSignInService)
    {
        _userSignInService = userSignInService;
    }

    public async Task<Result<AuthResponse>> Handle(LocalSignInCommand request, CancellationToken cancellationToken)
    {
        var authResult = await _userSignInService.SignInAsync(request.Creds);
        if (authResult.IsError)
            return authResult.Errors;

        return authResult.Value.ToResponse();
    }
}