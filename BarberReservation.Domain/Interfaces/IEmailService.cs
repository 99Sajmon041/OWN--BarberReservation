namespace BarberReservation.Domain.Interfaces;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string email, string token, CancellationToken cancellationToken);
}
