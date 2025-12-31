using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Commands.Self.DeactivateAccount;

public sealed class DeactivateAccountHandler(
    ILogger<DeactivateAccountHandler> logger,
    ICurrentAppUser currentAppUser,
    UserManager<ApplicationUser> userManager) : IRequestHandler<DeactivateAccountCommand>
{
    public async Task<Unit> Handle(DeactivateAccountCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var appUser = currentAppUser.User ?? throw new UnauthorizedException("Uživatel nebyl rozpoznán.");

        appUser.IsActive = false;

        var updateResult = await userManager.UpdateAsync(appUser);
        if(!updateResult.Succeeded)
        {
            var errors = new Dictionary<string, string[]>();
            errors["error"] = updateResult.Errors.Select(e => e.Description).ToArray();

            logger.LogError("Deactivation of the user failed, error: {Errors}", string.Join(", ", errors["error"]));
            throw new ValidationException("Operace selhala.", errors);
        }

        logger.LogInformation("User with ID: {UserId} was deactivated successfully.", appUser.Id);

        return Unit.Value;
    }
}
