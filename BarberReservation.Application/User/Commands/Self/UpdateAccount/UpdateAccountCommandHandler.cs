using AutoMapper;
using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Commands.Self.UpdateAccount;

public sealed class UpdateAccountCommandHandler(
    ILogger<UpdateAccountCommandHandler> logger,
    UserManager<ApplicationUser> userManager,
    IMapper mapper,
    ICurrentAppUser currentAppUser) : IRequestHandler<UpdateAccountCommand>
{
    public async Task<Unit> Handle(UpdateAccountCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var user = currentAppUser.User;

        mapper.Map(request, user);

        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            var errors = new Dictionary<string, string[]>
            {
                ["error"] = updateResult.Errors.Select(e => e.Description).ToArray()
            };

            logger.LogWarning("Profile update failed. Errors: {Errors}", string.Join(", ", errors["error"]));
            throw new ValidationException("Aktualizace profilu se nezdařila.", errors);
        }

        logger.LogInformation("User with ID: {UserId} updated profile successfully.", user.Id);

        return Unit.Value;
    }
}
