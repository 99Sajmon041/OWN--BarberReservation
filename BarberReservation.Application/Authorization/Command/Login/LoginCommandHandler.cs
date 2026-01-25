using BarberReservation.Application.Authorization.Jwt;
using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Entities;
using BarberReservation.Shared.Models.Authorization;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Authorization.Command.Login;

public sealed class LoginCommandHandler(
    ILogger<LoginCommandHandler> logger,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IJwtTokenService jwtTokenService) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null || !user.IsActive)
        {
            logger.LogWarning("Login failed - user not found. Email: {Email}.", request.Email);
            throw new UnauthorizedException("Neplatné přihlašovací údaje.");
        }

        var loginResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
        if(!loginResult.Succeeded)
        {
            logger.LogWarning("Login failed. Email: {Email}, LockedOut: {LockedOut}.", request.Email, loginResult.IsLockedOut);
            throw new UnauthorizedException("Neplatné přihlašovací údaje.");
        }

        var (token, expiresAt) = await jwtTokenService.CreateTokenAsync(user, request.RememberMe);

        logger.LogInformation("Login successful - Users e-mail: {UserEmail}, FullName: {UserFullName}.", request.Email, user.FullName);

        return new LoginResponse
        {
            Token = token,
            ExpiresAt = expiresAt,
            MustChangePassword = user.MustChangePassword
        };
    }
}
