using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Commands.Admin.DeactivateUser;

public sealed class DeactivateUserCommandHandler(
    ILogger<DeactivateUserCommandHandler> logger,
    UserManager<ApplicationUser> userManager) : IRequestHandler<DeactivateUserCommand>
{
    public async Task<Unit> Handle(DeactivateUserCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var user = await userManager.FindByIdAsync(request.Id);
        if (user is null)
        {
            logger.LogWarning("User with ID: {UserId} was not found.", request.Id);
            throw new NotFoundException("Uživatel nebyl nalezen.");
        }

        if(!user.IsActive)
        {
            logger.LogInformation("User with ID: {UserId} is already deactivated.", request.Id);
            return Unit.Value;
        }

        user.IsActive = false;

        var updateResult = await userManager.UpdateAsync(user);
        if(!updateResult.Succeeded)
        {
            var errors = new Dictionary<string, string[]>
            {
                ["error"] = updateResult.Errors.Select(e => e.Description).ToArray()
            };

            logger.LogError("Deactivation of the user failed. Error: {Errors}", string.Join(", ", errors["error"]));
            throw new BarberReservation.Application.Exceptions.ValidationException("Deaktivace uživatele selhala.", errors);
        }

        await userManager.UpdateSecurityStampAsync(user);
        logger.LogInformation("User with ID: {UserId} was deactivated.", request.Id);

        return Unit.Value;
    }
}
