namespace BarberReservation.Domain.Interfaces;

public interface IHairdresserServiceRepository
{
    Task<bool> ExistsByServiceIdAsync(int serviceId, CancellationToken ct);
}
