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
    public async Task Handle(UpdateAccountCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var user = currentAppUser.User;

        mapper.Map(request, user);

        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            var error = string.Join(", ", updateResult.Errors.Select(e => e.Description));

            logger.LogWarning("Profile update failed. Errors: {Errors}", error);
            throw new ValidationException("Chyba: " + error);
        }

        logger.LogInformation("User with ID: {UserId} updated profile successfully.", user.Id);
    }
}
