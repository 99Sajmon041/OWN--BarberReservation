using BarberReservation.Application.Authorization.Command.ForgotPassword;
using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

public sealed class ForgotPasswordCommandHandler(
    ILogger<ForgotPasswordCommandHandler> logger,
    UserManager<ApplicationUser> userManager,
    IEmailService emailService) : IRequestHandler<ForgotPasswordCommand>
{
    public async Task Handle(ForgotPasswordCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        logger.LogInformation("Password reset requested for email: {Email}.", request.Email);

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null || !user.IsActive)
        {
            logger.LogInformation("Password reset request acknowledged for email: {Email}.", request.Email);
            return;
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        await emailService.SendPasswordResetEmailAsync(user.Email!, token, ct);

        logger.LogInformation("Password reset instructions sent to: {Email}.", request.Email);
    }
}
