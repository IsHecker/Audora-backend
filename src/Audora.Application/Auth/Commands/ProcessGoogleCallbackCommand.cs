using Audora.Application.Auth.Configurations;
using Audora.Application.Auth.DTOs;
using Audora.Application.Common.Abstractions.Interfaces.Services;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;

namespace Audora.Application.Auth.Commands;

public record ProcessGoogleCallbackCommand(string State, string Code, GoogleAuthOptions Options) : ICommand;

public class ProcessGoogleCallbackCommandHandler : ICommandHandler<ProcessGoogleCallbackCommand>
{
    private readonly IGoogleAuthService _googleAuthService;
    private readonly IUserSignInService _userSignInService;
    private readonly IAuthResultStore _authResultStore;

    public ProcessGoogleCallbackCommandHandler(
        IGoogleAuthService googleAuthService,
        IUserSignInService userSignInService,
        IAuthResultStore authResultStore)
    {
        _googleAuthService = googleAuthService;
        _userSignInService = userSignInService;
        _authResultStore = authResultStore;
    }

    public async Task<Result> Handle(ProcessGoogleCallbackCommand request, CancellationToken cancellationToken)
    {
        if (!_authResultStore.ContainsState(request.State))
            return Error.Validation(description: "Invalid state");

        var options = request.Options;

        // get id_token
        var googleTokenDto = await _googleAuthService.ExchangeCodeAsync(
                options.TokenUrl,
                request.Code,
                options.RedirectUri,
                options.ClientId,
                options.ClientSecret);

        // validate id_token and extract user's creds from it
        var userCreds = await _googleAuthService.ValidateAndExtractIdTokenAsync(
                options.CertsUrl,
                googleTokenDto.IdToken,
                options.Issuer,
                aud: options.ClientId);

        var isEmailExisting = await _userSignInService.IsEmailExistingAsync(userCreds.Email);
        if (!isEmailExisting)
        {
            _authResultStore.SaveAuthResult(request.State, new AuthResult
            {
                IdToken = googleTokenDto.IdToken,
                RequiresRoleSelection = true
            });

            return Result.Success;
        }

        var authResult = await _userSignInService.SignInAsync(userCreds);

        if (authResult.IsError)
        {
            _authResultStore.SaveAuthResult(request.State, new AuthResult
            {
                Errors = authResult.Errors
            });

            return authResult.Errors;
        }

        _authResultStore.SaveAuthResult(request.State, authResult.Value);
        return Result.Success;
    }
}