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
    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        logger.LogInformation("Password reset requested for email: {Email}.", request.Email);

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Unit.Value;
        }

        var token = WebUtility.UrlDecode(request.Token);

        var result = await userManager.ResetPasswordAsync(user, token, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = new Dictionary<string, string[]>
            {
                ["error"] = result.Errors.Select(e => e.Description).ToArray()
            };

            logger.LogWarning("Password reset failed for user {UserId} ({Email}).", user.Id, user.Email);
            throw new ValidationException("Reset hesla se nezdařil.", errors);
        }

        user.MustChangePassword = false;
        
        var updateResult = await userManager.UpdateAsync(user);
        if(!updateResult.Succeeded)
        {
            var errors = new Dictionary<string, string[]>
            {
                ["error"] = updateResult.Errors.Select(e => e.Description).ToArray()
            };

            logger.LogError("Update user after password reset failed. Error: {Errors}", string.Join(", ", errors["error"]));
            throw new BarberReservation.Application.Exceptions.ValidationException("Operace selhala.", errors);
        }

        logger.LogInformation("Password reset succeeded for user {UserId} ({Email}).", user.Id, user.Email);

        return Unit.Value;
    }
}
