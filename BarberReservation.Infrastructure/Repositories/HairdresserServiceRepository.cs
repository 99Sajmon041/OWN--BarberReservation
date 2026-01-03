using BarberReservation.Domain.Interfaces;
using BarberReservation.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BarberReservation.Infrastructure.Repositories;

public sealed class HairdresserServiceRepository(BarberDbContext context) : IHairdresserServiceRepository
{
    private readonly BarberDbContext _context = context;

    public async Task<bool> ExistsByServiceIdAsync(int serviceId, CancellationToken ct)
    {
        return await _context.HairdresserServices
            .AsNoTracking()
            .AnyAsync(x => x.ServiceId == serviceId && x.IsActive, ct);

    }
}
