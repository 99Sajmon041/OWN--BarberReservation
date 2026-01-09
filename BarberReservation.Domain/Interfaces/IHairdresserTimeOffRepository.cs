namespace BarberReservation.Domain.Interfaces;

public interface IHairdresserTimeOffRepository
{
    Task<bool> ExistsOverlapAsync(string hairdresserId, DateTime startAt, DateTime endAt, CancellationToken ct);
}
