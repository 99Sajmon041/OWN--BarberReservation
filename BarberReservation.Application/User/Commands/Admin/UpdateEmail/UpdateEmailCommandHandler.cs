using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Commands.Admin.UpdateEmail;

public sealed class UpdateEmailCommandHandler(
    ILogger<UpdateEmailCommandHandler> logger,
    UserManager<ApplicationUser> userManager) : IRequestHandler<UpdateEmailCommand>
{
    public async Task<Unit> Handle(UpdateEmailCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var user = await userManager.FindByIdAsync(request.Id);
        if (user is null)
        {
            logger.LogWarning("Update e-mail failed. User with ID {UserId} was not found.", request.Id);
            throw new NotFoundException("Uživatel nebyl nalezen.");
        }

        var newEmail = request.Email.Trim();

        if (string.Equals(user.Email, newEmail, StringComparison.OrdinalIgnoreCase))
        {
            logger.LogInformation("Update email skipped. Email is already set for user {UserId}.", user.Id);
            return Unit.Value;
        }

        var existingUser = await userManager.FindByEmailAsync(newEmail);
        if (existingUser is not null && existingUser.Id != user.Id)
        {
            logger.LogWarning("Update email failed. Email {Email} is already used by another user.", newEmail);
            throw new ConflictException("E-mail již existuje, zvolte unikátní e-mail.");
        }

        var setEmailResult = await userManager.SetEmailAsync(user, newEmail);
        if (!setEmailResult.Succeeded)
        {
            var error = string.Join(", ", setEmailResult.Errors.Select(e => e.Description));

            logger.LogWarning("SetEmail failed for user {UserId}. Error: {Errors}", user.Id, error);
            throw new ValidationException("Chyba: " + error);
        }

        var setUserNameResult = await userManager.SetUserNameAsync(user, newEmail);
        if (!setUserNameResult.Succeeded)
        {
            var error = string.Join(", ", setUserNameResult.Errors.Select(e => e.Description));

            logger.LogWarning("SetUserName failed for user {UserId}. Error: {Errors}",user.Id, error);
            throw new ValidationException("Chyba: " + error);
        }

        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            var error = string.Join(", ", updateResult.Errors.Select(e => e.Description));

            logger.LogWarning("Update user failed after email change for user {UserId}. Error: {Errors}", user.Id, error);
            throw new ValidationException("Chyba: " + error);
        }

        await userManager.UpdateSecurityStampAsync(user);

        logger.LogInformation("Email changed successfully for user {UserId} to {Email}.", user.Id, newEmail);
        return Unit.Value;
    }
}
