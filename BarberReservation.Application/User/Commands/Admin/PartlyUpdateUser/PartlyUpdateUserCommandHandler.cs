using AutoMapper;
using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Commands.Admin.PartlyUpdateUser;

public sealed class PartlyUpdateUserCommandHandler(
    ILogger<PartlyUpdateUserCommandHandler> logger,
    UserManager<ApplicationUser> userManager,
    IMapper mapper,
    ICurrentAppUser currentAppUser) : IRequestHandler<PartlyUpdateUserCommand>
{
    public async Task<Unit> Handle(PartlyUpdateUserCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var user = await userManager.FindByIdAsync(request.Id);
        if (user is null)
        {
            logger.LogWarning("User with ID: {UserId} was not found.", request.Id);
            throw new NotFoundException("Uživatel nebyl nalezen.");
        }

        if(currentAppUser.User.Id == user.Id)
        {
            logger.LogWarning("Admin tries update own profile. Not allowed.");
            throw new ForbiddenException("Nelze upravit svůj administrátorský účet.");
        }

        mapper.Map(request, user);

        var updateResult = await userManager.UpdateAsync(user);
        if(!updateResult.Succeeded)
        {
            var error = string.Join(", ", updateResult.Errors.Select(e => e.Description));

            logger.LogError("Failed to update user. Error: {Errors}", error);
            throw new ValidationException("Chyba: " + error);
        }

        await userManager.UpdateSecurityStampAsync(user);

        logger.LogInformation("User with ID: {UserId} was updated successfuly.", user.Id);

        return Unit.Value;
    }
}
