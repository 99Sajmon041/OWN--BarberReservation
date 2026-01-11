namespace BarberReservation.Domain.Interfaces;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string email, string token, CancellationToken ct);
    Task SendReservationConfirmationEmailAsync(string email,
        DateTime reservationTime,
        string hairdresserName,
        string serviceName,
        decimal price,
        int durationTime,
        CancellationToken ct);
}