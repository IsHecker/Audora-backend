using Audora.Application.Auth.DTOs;
using Audora.Application.Common.Abstractions.Interfaces.Services;
using Audora.Application.Common.Results;
using Audora.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Audora.Infrastructure.Services;

public class UserSignInService : IUserSignInService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly TokenGeneratorService _tokenGenerator;

    public UserSignInService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        TokenGeneratorService tokenGenerator,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenGenerator = tokenGenerator;
        _roleManager = roleManager;
    }

    public async Task<bool> IsEmailExistingAsync(string email)
    {
        return (await _userManager.FindByEmailAsync(email)) is not null;
    }

    public async Task<Result<AuthResult>> RegisterAsync(UserCredentialsDto credentials)
    {
        var user = await _userManager.FindByEmailAsync(credentials.Email);

        if (user is not null)
            return Error.Conflict(description: "There's an account with this Email");


        var userCreationResult = await CreateUserAsync(credentials);
        if (userCreationResult.IsError)
            return userCreationResult.Errors;

        user = userCreationResult.Value;
        if (credentials.Provider is not null)
            await AddExternalUserAsync(credentials, user);

        return _tokenGenerator.GenerateToken(credentials, user.Id);
    }

    public async Task<Result<AuthResult>> SignInAsync(UserCredentialsDto credentials)
    {
        // Handle both external and local cases
        // Look up user by email or provider id
        // Register new if needed
        // Generate JWT and return

        var user = await _userManager.FindByEmailAsync(credentials.Email);

        if (user is null)
            return Error.Unauthorized(description: "Invalid email or password.");

        if (credentials.Provider is not null && !await IsExternalAccount(credentials))
            return Error.Conflict(
                description: "An account with this email already exists. Please sign in using email and password.");

        if (credentials.Password is not null)
        {
            var isAuthenticated = await AuthenticateLocalUserAsync(credentials, user);
            if (!isAuthenticated)
                return Error.Unauthorized(description: "Invalid email or password.");
        }

        credentials.Role = (await _userManager.GetRolesAsync(user)).First();

        return _tokenGenerator.GenerateToken(credentials, user.Id);
    }

    private async Task AddExternalUserAsync(UserCredentialsDto credentials, ApplicationUser user)
    {
        var provider = credentials.Provider!;
        var providerId = credentials.ProviderUserId!;

        var newLogin = new UserLoginInfo(provider, providerId, provider);
        await _userManager.AddLoginAsync(user, newLogin);
    }

    private async Task<bool> AuthenticateLocalUserAsync(UserCredentialsDto credentials, ApplicationUser user)
    {
        return (await _signInManager.CheckPasswordSignInAsync(user, credentials.Password, lockoutOnFailure: false))
            .Succeeded;
    }

    private async Task<Result<ApplicationUser>> CreateUserAsync(UserCredentialsDto credentials)
    {
        var newUser = new ApplicationUser
        {
            FullName = credentials.FullName!,
            UserName = credentials.Email,
            Email = credentials.Email,
            EmailConfirmed = true,
            PictureUrl = credentials.ProfilePictureUrl
        };

        IdentityResult userCreationResult;
        if (credentials.Password is not null)
            userCreationResult = await _userManager.CreateAsync(newUser, credentials.Password);
        else
            userCreationResult = await _userManager.CreateAsync(newUser);

        if (!userCreationResult.Succeeded)
            return Error.Validation(description: string.Join(", ", userCreationResult.Errors.Select(e => new { e.Code, e.Description })));

        if (!await _roleManager.RoleExistsAsync(credentials.Role!))
            return Error.Validation(description: $"Role '{credentials.Role}' does not exist.");

        var result = await _userManager.AddToRoleAsync(newUser, credentials.Role!);
        if (!result.Succeeded)
            return Error.Validation(description: string.Join(", ", result.Errors));

        return newUser;
    }

    private async Task<bool> IsExternalAccount(UserCredentialsDto credentials)
    {
        return await _userManager.FindByLoginAsync(credentials.Provider!, credentials.ProviderUserId!) is not null;
    }
}