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

    public async Task<IReadOnlyList<HairdresserWorkingHours>> GetWeekAsync(
    string hairdresserId,
    DateOnly onDate,
    bool includeHairdresser,
    bool tracked,
    CancellationToken ct)
    {
        IQueryable<HairdresserWorkingHours> baseQuery = _context.HairdresserWorkingHours;

        if (!tracked)
            baseQuery = baseQuery.AsNoTracking();

        if (includeHairdresser)
            baseQuery = baseQuery.Include(x => x.Hairdresser);

        var effectiveFrom = await baseQuery
            .Where(x => x.HairdresserId == hairdresserId && x.EffectiveFrom <= onDate)
            .MaxAsync(x => (DateOnly?)x.EffectiveFrom, ct);

        if (effectiveFrom is null)
            return [];

        return await baseQuery
            .Where(x => x.HairdresserId == hairdresserId && x.EffectiveFrom == effectiveFrom.Value)
            .OrderBy(x => x.DayOfWeek)
            .ToListAsync(ct);
    }

    public void AddDaysToWorkingWeek(IEnumerable<HairdresserWorkingHours> days)
    {
        _context.HairdresserWorkingHours.AddRange(days);
    }

    public async Task<List<HairdresserWorkingHours>> GetWeekByEffectiveFromAsync(string hairdresserId, DateOnly validFromDate, CancellationToken ct)
    {
        return await _context.HairdresserWorkingHours
            .Where(x => x.HairdresserId == hairdresserId && x.EffectiveFrom == validFromDate)
            .OrderBy(x => x.DayOfWeek)
            .ToListAsync(ct);
    }
}
