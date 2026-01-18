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
    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var current = currentAppUser.User;

        var result = await userManager.ChangePasswordAsync(current, request.OldPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = new Dictionary<string, string[]>
            {
                ["error"] = result.Errors.Select(e => e.Description).ToArray()
            };

            logger.LogWarning("Attempt to change password failed for user {UserId}. Errors: {Errors}", current.Id, string.Join(", ", errors["error"]));

            throw new ValidationException("Změna hesla se nezdařila.", errors);
        }

        if(current.MustChangePassword)
        {
            current.MustChangePassword = false;
            await userManager.UpdateAsync(current);
        }

        return Unit.Value;
    }
}
