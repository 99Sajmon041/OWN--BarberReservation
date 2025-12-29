using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Commands.Self.DeactivateAccount;

public sealed class DeactivateAccountHandler(
    ILogger<DeactivateAccountHandler> logger,
    IUserContext userContext,
    UserManager<ApplicationUser> userManager) : IRequestHandler<DeactivateAccountCommand>
{
    public async Task<Unit> Handle(DeactivateAccountCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var user = userContext.GetCurrentUser() ?? throw new UnauthorizedException("Uživatel nebyl rozpoznán.");

        var appUser = await userManager.FindByIdAsync(user.Id) ?? throw new UnauthorizedException("Uživatel nebyl nalezen.");

        if (!appUser.IsActive)
        {
            logger.LogInformation("User with ID: {UserId} is already inactive. No action taken.", user.Id);
            return Unit.Value;
        }

        appUser.IsActive = false;

        var updateresult = await userManager.UpdateAsync(appUser);
        if(!updateresult.Succeeded)
        {
            var errors = new Dictionary<string, string[]>();
            errors["error"] = updateresult.Errors.Select(e => e.Description).ToArray();

            logger.LogError("Deactivation of the user failed, error: {Errors}", string.Join(", ", errors["error"]));
            throw new ValidationException("Operace selhala.", errors);
        }

        logger.LogInformation("User with ID: {UserId} was deactivated successfully.", user.Id);

        return Unit.Value;
    }
}
