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
    IUserContext userContext) : IRequestHandler<ChangePasswordCommand>
{
    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var appUser = userContext.GetCurrentUser();
        if (appUser is null)
        {
            logger.LogWarning("Attempt to change password failed: current user not found in context.");
            throw new UnauthorizedException("Uživatel není přihlášen.");
        }

        var user = await userManager.FindByEmailAsync(appUser.Email);
        if (user is null)
        {
            logger.LogWarning("Attempt to change password failed: user not found by email {UserEmail}.", appUser.Email);
            throw new NotFoundException("Uživatel nebyl nalezen.");
        }

        var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = ErrorBuilder.SetErrorMessage(result.Errors);
            logger.LogWarning("Attempt to change password failed for user {UserId}: {Errors}", user.Id, errors);

            throw new DomainException(errors);
        }

        return Unit.Value;
    }
}
