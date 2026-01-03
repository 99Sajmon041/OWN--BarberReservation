using BarberReservation.Domain.Interfaces;
using BarberReservation.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BarberReservation.Infrastructure.Repositories;

public sealed class ReservationRepository(BarberDbContext context) : IReservationRepository
{
    private readonly BarberDbContext _context = context;

    public async Task<bool> ExistsByHairDresserServiceIdAsync(int hairDresserServiceId, CancellationToken ct)
    {
        return await _context.Reservations.AnyAsync(x => x.HairdresserServiceId == hairDresserServiceId, ct);
    }
}
