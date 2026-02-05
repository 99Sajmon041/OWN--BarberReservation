using AutoMapper;
using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Entities;
using BarberReservation.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Authorization.Command.Register;

public sealed class RegisterCommandHandler(
    ILogger<RegisterCommandHandler> logger,
    UserManager<ApplicationUser> userManager,
    IMapper mapper) : IRequestHandler<RegisterCommand>
{
    public async Task Handle(RegisterCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var user = mapper.Map<ApplicationUser>(request);
        user.IsActive = true;
        user.MustChangePassword = false;

        var result = await userManager.CreateAsync(user, request.Password);
        if(!result.Succeeded)
        {
            var errors = ErrorBuilder.SetErrorMessage(result.Errors);

            logger.LogWarning("Failed to create user, Error: {Errrors}", errors);
            throw new DomainException(errors);
        }

        var roleResult = await userManager.AddToRoleAsync(user, nameof(UserRoles.Customer));
        if (!roleResult.Succeeded)
        {
            var errors = ErrorBuilder.SetErrorMessage(roleResult.Errors);

            logger.LogWarning("Failed to assign role to user, Error: {Errrors}", errors);
            throw new DomainException(errors);
        }

        logger.LogInformation("User: {FullName}, with e-mail: {Email} was created successfuly.", user.FullName, user.Email);
    }
}
