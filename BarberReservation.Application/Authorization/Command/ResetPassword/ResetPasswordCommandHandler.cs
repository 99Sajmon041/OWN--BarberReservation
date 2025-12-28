using BarberReservation.Application;
using BarberReservation.Application.Authorization.Command.ResetPassword;
using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Net;

public sealed class ResetPasswordCommandHandler(
    ILogger<ResetPasswordCommandHandler> logger,
    UserManager<ApplicationUser> userManager) : IRequestHandler<ResetPasswordCommand>
{
    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        logger.LogInformation("Password reset requested for email: {Email}.", request.Email);

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            logger.LogInformation("Password reset failed (invalid/expired link) for email: {Email}.", request.Email);
            throw new DomainException("Odkaz pro reset hesla je neplatný nebo vypršel.");
        }

        var token = WebUtility.UrlDecode(request.Token);

        var result = await userManager.ResetPasswordAsync(user, token, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = ErrorBuilder.SetErrorMessage(result.Errors);

            logger.LogWarning("Password reset failed for user {UserId} ({Email}). Errors: {Errors}.", user.Id, user.Email, errors);

            throw new DomainException("Nepodařilo se změnit heslo.");
        }

        logger.LogInformation("Password reset succeeded for user {UserId} ({Email}).", user.Id, user.Email);

        return Unit.Value;
    }
}
