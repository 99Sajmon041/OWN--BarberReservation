using AutoMapper;
using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Commands.Admin.PartlyUpdateUser;

public sealed class PartlyUpdateUserCommandHandler(
    ILogger<PartlyUpdateUserCommandHandler> logger,
    UserManager<ApplicationUser> userManager,
    IMapper mapper) : IRequestHandler<PartlyUpdateUserCommand>
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

        mapper.Map(request, user);

        var updateResult = await userManager.UpdateAsync(user);
        if(!updateResult.Succeeded)
        {
            var errors = new Dictionary<string, string[]>
            {
                ["error"] = updateResult.Errors.Select(e => e.Description).ToArray()
            };

            logger.LogError("Failed to update user. Error: {Errors}", string.Join(", ", errors["error"]));
            throw new ValidationException(errors);
        }

        logger.LogInformation("User with ID: {UserId} was updated successfuly.", user.Id);

        return Unit.Value;
    }
}
