using AutoMapper;
using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Commands.Admin.CreateUser;

public sealed class CreateUserCommandHandler(
    ILogger<CreateUserCommandHandler> logger,
    UserManager<ApplicationUser> userManager,
    IEmailService emailService,
    IMapper mapper) : IRequestHandler<CreateUserCommand>
{
    public async Task<Unit> Handle(CreateUserCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var user = mapper.Map<ApplicationUser>(request);

        user.IsActive = true;
        user.MustChangePassword = true;
        user.EmailConfirmed = true;
        user.UserName = user.Email;

        var createResult = await userManager.CreateAsync(user);
        if(!createResult.Succeeded)
        {
            var errors = new Dictionary<string, string[]>
            {
                ["error"] = createResult .Errors.Select(e => e.Description).ToArray()
            };

            logger.LogError("Create user failed. Error: {Errors}", string.Join(", ", errors["error"]));
            throw new BarberReservation.Application.Exceptions.ValidationException("Vytvoření uživatele selhalo.", errors);
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        await emailService.SendPasswordResetEmailAsync(user.Email!, token, ct);

        logger.LogInformation("User {UserId} ({Email}) created. Reset email sent.", user.Id, user.Email);


        return Unit.Value;
    }
}
