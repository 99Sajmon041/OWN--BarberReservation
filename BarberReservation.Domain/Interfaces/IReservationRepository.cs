namespace BarberReservation.Domain.Interfaces;

public interface IReservationRepository
{
    Task<bool> ExistsByHairDresserServiceIdAsync(int hairDresserServiceid, CancellationToken ct);
}
