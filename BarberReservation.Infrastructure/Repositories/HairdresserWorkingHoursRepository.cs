using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BarberReservation.Infrastructure.Repositories;

public sealed class HairdresserWorkingHoursRepository(BarberDbContext context) : IHairdresserWorkingHoursRepository
{
    private readonly BarberDbContext _context = context;

    public async Task<HairdresserWorkingHours?> GetForDayAsync(string hairdresserId, DayOfWeek dayOfWeek, CancellationToken ct)
    {
        return await _context.HairdresserWorkingHours
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.HairdresserId == hairdresserId && x.DayOfWeek == dayOfWeek, ct);
    }
}
