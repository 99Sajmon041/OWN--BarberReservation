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

        var current = userContext.GetCurrentUser() ?? throw new UnauthorizedException("Uživatel není přihlášen.");

        var user = await userManager.FindByIdAsync(current.Id) ?? throw new UnauthorizedException("Uživatel nebyl nalezen.");

        var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = new Dictionary<string, string[]>
            {
                ["error"] = result.Errors.Select(e => e.Description).ToArray()
            };

            logger.LogWarning("Attempt to change password failed for user {UserId}. Errors: {Errors}", user.Id, string.Join(", ", errors["error"]));

            throw new ValidationException("Změna hesla se nezdařila.", errors);
        }

        return Unit.Value;
    }
}
