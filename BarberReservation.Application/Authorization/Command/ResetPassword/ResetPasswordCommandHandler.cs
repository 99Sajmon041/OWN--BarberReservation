using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Net;

namespace BarberReservation.Application.Authorization.Command.ResetPassword;

public sealed class ResetPasswordCommandHandler(
    ILogger<ResetPasswordCommandHandler> logger,
    UserManager<ApplicationUser> userManager) : IRequestHandler<ResetPasswordCommand>
{
    public async Task Handle(ResetPasswordCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        logger.LogInformation("Password reset requested for email: {Email}.", request.Email);

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return;
        }

        var token = request.Token;

        var resetPwdResult = await userManager.ResetPasswordAsync(user, token, request.NewPassword);
        if (!resetPwdResult.Succeeded)
        {
            var error = string.Join(", ", resetPwdResult.Errors.Select(e => e.Description));

            logger.LogWarning("Password reset failed for user {UserId} ({Email}). Error: {Error}", user.Id, user.Email, error);
            throw new ValidationException("Chyba :" + error);
        }

        user.MustChangePassword = false;
        
        var updateResult = await userManager.UpdateAsync(user);
        if(!updateResult.Succeeded)
        {
            var error = string.Join(", ", updateResult.Errors.Select(e => e.Description));

            logger.LogError("Update user after password reset failed. Error: {Errors}", error);
            throw new ValidationException("Chyba :" + error);
        }

        logger.LogInformation("Password reset succeeded for user {UserId} ({Email}).", user.Id, user.Email);
    }
}
