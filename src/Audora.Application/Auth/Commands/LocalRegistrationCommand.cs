using Audora.Application.Auth.DTOs;
using Audora.Application.Common.Abstractions.Interfaces.Services;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Results;
using Audora.Contracts.Auth.Responses;

namespace Audora.Application.Auth.Commands;

public record LocalRegistrationCommand(UserCredentialsDto Creds) : ICommand<AuthResponse>;

public class LocalRegistrationCommandHandler : ICommandHandler<LocalRegistrationCommand, AuthResponse>
{
    private readonly IUserSignInService _userSignInService;

    public LocalRegistrationCommandHandler(
        IUserSignInService userSignInService)
    {
        _userSignInService = userSignInService;
    }

    public async Task<Result<AuthResponse>> Handle(LocalRegistrationCommand request, CancellationToken cancellationToken)
    {
        var authResult = await _userSignInService.RegisterAsync(request.Creds);
        if (authResult.IsError)
            return authResult.Errors;

        return authResult.Value.ToResponse();
    }
}