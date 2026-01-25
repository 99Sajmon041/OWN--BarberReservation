using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Commands.Admin.ActivateUser;

public sealed class ActivateUserCommandHandler(
    ILogger<ActivateUserCommandHandler> logger,
    UserManager<ApplicationUser> userManager) : IRequestHandler<ActivateUserCommand>
{
    public async Task<Unit> Handle(ActivateUserCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var user = await userManager.FindByIdAsync(request.Id);
        if (user is null)
        {
            logger.LogWarning("User with ID: {UserId} was not found.", request.Id);
            throw new NotFoundException("Uživatel nebyl nalezen.");
        }

        if (user.IsActive)
            return Unit.Value;

        user.IsActive = true;

        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            var error = string.Join(", ", updateResult.Errors.Select(e => e.Description));
            logger.LogError("Activation of the user failed. Error: {Errors}", error);
            throw new ValidationException("Chyba: " + error);
        }

        await userManager.UpdateSecurityStampAsync(user);
        logger.LogInformation("User with ID: {UserId} was activated.", user.Id);

        return Unit.Value;
    }
}
