using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Authorization.Command.ChangePassword;

public sealed class ChangePasswordCommandHandler(
    ILogger<ChangePasswordCommandHandler> logger,
    UserManager<ApplicationUser> userManager,
    ICurrentAppUser currentAppUser) : IRequestHandler<ChangePasswordCommand>
{
    public async Task Handle(ChangePasswordCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var current = currentAppUser.User;

        var changePwdResult = await userManager.ChangePasswordAsync(current, request.OldPassword, request.NewPassword);
        if (!changePwdResult.Succeeded)
        {
            var error = changePwdResult.Errors.Select(e => e.Description);

            logger.LogWarning("Attempt to change password failed for user with ID: {UserId}. Errors: {Errors}", current.Id, error);

            throw new ValidationException("Chyba: " + error);
        }

        if(current.MustChangePassword)
        {
            current.MustChangePassword = false;
            await userManager.UpdateAsync(current);
        }
    }
}
