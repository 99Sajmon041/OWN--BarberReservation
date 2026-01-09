using BarberReservation.Domain.Interfaces;
using BarberReservation.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BarberReservation.Infrastructure.Repositories;

public sealed class HairdresserTimeOffRepository(BarberDbContext context) : IHairdresserTimeOffRepository
{
    private readonly BarberDbContext _context = context;

    public async Task<bool> ExistsOverlapAsync(string hairdresserId, DateTime startAt, DateTime endAt, CancellationToken ct)
    {
        return await _context.HairdresserTimeOffs
            .AsNoTracking()
            .AnyAsync(x => x.HairdresserId == hairdresserId && (x.StartAt < endAt && x.EndAt > startAt), ct);
    }
}