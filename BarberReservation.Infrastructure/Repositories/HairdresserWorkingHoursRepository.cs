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
            .FirstOrDefaultAsync(x => x.HairdresserId == hairdresserId && x.DayOfWeek == dayOfWeek && x.IsWorkingDay, ct);
    }

    public async Task<IReadOnlyList<HairdresserWorkingHours>> GetAllDaysInWeekForHairdresser(
        string hairdresserId, 
        bool tracked, bool includeHairdresser, 
        CancellationToken ct)
    {
        IQueryable<HairdresserWorkingHours> query = _context.HairdresserWorkingHours;

        if (!tracked)
            query = query.AsNoTracking();

        if (includeHairdresser)
            query = query.Include(x => x.Hairdresser);

        return await query
            .Where(x => x.HairdresserId == hairdresserId)
            .OrderBy(x => x.DayOfWeek)
            .ToListAsync(ct);
    }

    public void AddDayToWorkingWeek(HairdresserWorkingHours day)
    {
        _context.HairdresserWorkingHours.Add(day);
    }
}
