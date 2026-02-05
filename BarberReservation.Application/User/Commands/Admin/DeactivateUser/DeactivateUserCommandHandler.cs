using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Commands.Admin.DeactivateUser;

public sealed class DeactivateUserCommandHandler(
    ILogger<DeactivateUserCommandHandler> logger,
    UserManager<ApplicationUser> userManager,
    IUnitOfWork unitOfWork,
    ICurrentAppUser currentAppUser) : IRequestHandler<DeactivateUserCommand>
{
    public async Task Handle(DeactivateUserCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var user = await userManager.FindByIdAsync(request.Id);
        if (user is null)
        {
            logger.LogWarning("User with ID: {UserId} was not found.", request.Id);
            throw new NotFoundException("Uživatel nebyl nalezen.");
        }

        if (currentAppUser.User.Id == request.Id)
        {
            logger.LogWarning("Admin tried to deactivate self. UserId: {UserId}", request.Id);
            throw new ValidationException("Nemůžete deaktivovat sami sebe.");
        }

        if (!user.IsActive)
        {
            logger.LogInformation("User with ID: {UserId} is already deactivated.", request.Id);
            return;
        }

        if (await userManager.IsInRoleAsync(user, nameof(UserRoles.Admin)))
        {
            logger.LogWarning("User with ID: {UserId} is admin and cannot be deactivated.", request.Id);
            throw new ValidationException("Není povolené deaktivovat uživatele v roli admin.");
        }

        var isHairdresser = await userManager.IsInRoleAsync(user, nameof(UserRoles.Hairdresser));

        if (isHairdresser)
        {
            if (await unitOfWork.HairdresserServiceRepository.ExistsAnyByHairdresserAsync(user.Id, ct))
            {
                logger.LogWarning("Hairdresser with ID: {UserId} can not be deactivated. He has active hairdresser services.", request.Id);

                throw new ConflictException("Nelze deaktivovat kadeřníka, má stále aktivní své služby.");
            }

            if (await unitOfWork.ReservationRepository.ExistAnyUpComingReservationForHairdresserAsync(user.Id, ct))
            {
                logger.LogWarning("Hairdresser with ID: {UserId} can not be deactivated. He has active future reservations.", request.Id);

                throw new ConflictException("Nelze deaktivovat kadeřníka, má nadcházející rezervace.");
            }
        }

        if (await unitOfWork.ReservationRepository.ExistAnyUpComingReservationForClientAsync(user.Id, ct))
        {
            logger.LogWarning("User with ID: {UserId} has upcoming reservation. It is not allowed to deactivate them.", request.Id);

            throw new ConflictException("Není povolené deaktivovat uživatele, který má nadcházející rezervaci.");
        }

        user.IsActive = false;

        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            var error = string.Join(", ", updateResult.Errors.Select(e => e.Description));
            logger.LogError("Deactivation of the user failed. Error: {Errors}", error);
            throw new BarberReservation.Application.Exceptions.ValidationException("Chyba: " + error);
        }

        await userManager.UpdateSecurityStampAsync(user);
        logger.LogInformation("User with ID: {UserId} was deactivated.", request.Id);
    }
}
