using BarberReservation.Shared.Models.LookUpModels;

namespace BarberReservation.Domain.Interfaces;

public interface IReservationLookupsRepository
{
    Task<IReadOnlyList<ReservationClientLookUpDto>> SearchCustomersAsync(string term, int take, CancellationToken ct);
}
