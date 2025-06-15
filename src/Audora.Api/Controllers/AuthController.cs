using System.Security.Cryptography;
using Audora.Application.Auth.Commands;
using Audora.Application.Auth.Configurations;
using Audora.Application.Auth.Queries;
using Audora.Application.Common.Mappings;
using Audora.Contracts.Auth.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Audora.Api.Controllers;

public class AuthController : ApiController
{
    private readonly ISender _sender;
    private readonly GoogleAuthOptions _googleOptions;

    public AuthController(
        ISender sender,
        IOptions<GoogleAuthOptions> options)
    {
        _sender = sender;
        _googleOptions = options.Value;
    }


    [HttpGet(ApiEndpoints.Authentication.LoginWithGoogle)]
    public async Task<IActionResult> LoginWithGoogle()
    {
        var query = new GenerateGoogleUrlQuery(_googleOptions);
        var urlResult = await _sender.Send(query);
        return urlResult.Match(
            url => Ok(new { Url = url }),
            Problem);
    }

    [HttpGet(ApiEndpoints.Authentication.Callback)]
    public async Task<IActionResult> Callback(string state, string code)
    {
        var command = new ProcessGoogleCallbackCommand(state, code, _googleOptions);
        await _sender.Send(command);
        return PhysicalFile(Path.GetFullPath("wwwroot/google-auth-callback.html"), "text/html");
    }

    [HttpGet(ApiEndpoints.Authentication.AuthResult)]
    public async Task<IActionResult> AuthResult(string state)
    {
        var query = new GetGoogleAuthResultQuery(state);
        var authResult = await _sender.Send(query);
        return authResult.Match(Ok, Problem);
    }

    [HttpPost(ApiEndpoints.Authentication.RegisterWithGoogle)]
    public async Task<IActionResult> RegisterWithGoogle(GoogleRegisterRequest request)
    {
        var command = new CompleteGoogleRegistrationCommand(request.Role, request.IdToken, _googleOptions);

        var registrationResult = await _sender.Send(command);
        return registrationResult.Match(Ok, Problem);
    }

    [HttpPost(ApiEndpoints.Authentication.LoginLocal)]
    public async Task<IActionResult> LoginLocal(AuthRequest request)
    {
        var command = new LocalSignInCommand(request.ToUserCredentials());
        var signInResult = await _sender.Send(command);
        return signInResult.Match(Ok, Problem);
    }


    [HttpPost(ApiEndpoints.Authentication.RegisterLocal)]
    public async Task<IActionResult> RegisterLocal(AuthRequest request)
    {
        var command = new LocalRegistrationCommand(request.ToUserCredentials());
        var registrationResult = await _sender.Send(command);
        return registrationResult.Match(Ok, Problem);
    }

    private static void RefreshToken()
    {
        // client_id=YOUR_CLIENT_ID
        // client_secret=YOUR_CLIENT_SECRET
        // refresh_token=YOUR_REFRESH_TOKEN
        // grant_type=refresh_token
    }
}