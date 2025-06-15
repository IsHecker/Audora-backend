using Audora.Application.Auth.Configurations;
using Audora.Application.Common.Abstractions.Interfaces.Services;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Results;
using Audora.Contracts.Auth.Responses;

namespace Audora.Application.Auth.Commands;

public record CompleteGoogleRegistrationCommand(string Role, string IdToken, GoogleAuthOptions Options) : ICommand<AuthResponse>;

public class CompleteGoogleRegistrationCommandHandler : ICommandHandler<CompleteGoogleRegistrationCommand, AuthResponse>
{
    private readonly IGoogleAuthService _googleAuthService;
    private readonly IUserSignInService _userSignInService;

    public CompleteGoogleRegistrationCommandHandler(IGoogleAuthService googleAuthService, IUserSignInService userSignInService)
    {
        _googleAuthService = googleAuthService;
        _userSignInService = userSignInService;
    }

    public async Task<Result<AuthResponse>> Handle(CompleteGoogleRegistrationCommand request, CancellationToken cancellationToken)
    {
        var options = request.Options;

        // validate id_token and extract user's creds from it
        var userCreds = await _googleAuthService.ValidateAndExtractIdTokenAsync(
                options.CertsUrl,
                request.IdToken,
                options.Issuer,
                aud: options.ClientId);

        userCreds.Role = request.Role;

        var authResult = await _userSignInService.RegisterAsync(userCreds);

        if (authResult is null)
            return Error.Unexpected(description: "Token is Invalid!!!");

        return authResult.Value.ToResponse();
    }
}