using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Commands.Self.DeactivateAccount;

public sealed class DeactivateAccountHandler(
    ILogger<DeactivateAccountHandler> logger,
    ICurrentAppUser currentAppUser,
    UserManager<ApplicationUser> userManager,
    IUnitOfWork unitOfWork) : IRequestHandler<DeactivateAccountCommand>
{
    public async Task Handle(DeactivateAccountCommand request, CancellationToken ct)
    {
        var appUser = currentAppUser.User;

        var roles = await userManager.GetRolesAsync(appUser);
        if(roles.Contains(nameof(UserRoles.Admin)) || roles.Contains(nameof(UserRoles.Hairdresser)))
        {
            logger.LogWarning("User which is not customer tries to deactivete own account. User with ID: {UserId}", appUser.Id);
            throw new ConflictException("Deaktivace svého účtu je povolená pouze pro zákazníka.");
        }

        var existsUpComingReservation = await unitOfWork.ReservationRepository.ExistAnyUpComingReservationAsync(appUser.Id, ct);
        if (existsUpComingReservation)
        {
            logger.LogWarning("User tries to deactivate own profile but he has up coming reservation - access denied. User with ID: {UserId}", appUser.Id);
            throw new ConflictException("Deaktivace svého účtu je povolená pouze v případě, že neexistuje nadcházející rezervace. Nejprve ji zrušte.");
        }

        appUser.IsActive = false;

        var updateResult = await userManager.UpdateAsync(appUser);
        if(!updateResult.Succeeded)
        {
            var error = string.Join(", ", updateResult.Errors.Select(e => e.Description));

            logger.LogError("Deactivation of the user failed, error: {Errors}", error);
            throw new ValidationException("Chyba: " + error);
        }

        logger.LogInformation("User with ID: {UserId} was deactivated successfully.", appUser.Id);
    }
}
