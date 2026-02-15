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

    public async Task<IReadOnlyList<HairdresserWorkingHours>> GetCurrentWeekAsync(
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

    public async Task<IReadOnlyList<HairdresserWorkingHours>> GetNextWeekAsync(
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

        var nextEffectiveFrom = await baseQuery
            .Where(x => x.HairdresserId == hairdresserId && x.EffectiveFrom > onDate)
            .MinAsync(x => (DateOnly?)x.EffectiveFrom, ct);

        if (nextEffectiveFrom is null)
            return [];

        return await baseQuery
            .Where(x => x.HairdresserId == hairdresserId && x.EffectiveFrom == nextEffectiveFrom.Value)
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

    public async Task<HairdresserWorkingHours?> GetEffectiveFromDayByTimeOffAsync(string hairdresserId, DateOnly TimeOffDay, CancellationToken ct)
    {
        return await _context
            .HairdresserWorkingHours
            .AsNoTracking()
            .OrderByDescending(x =>  x.EffectiveFrom)
            .FirstOrDefaultAsync(
            x => x.DayOfWeek == TimeOffDay.DayOfWeek
            && x.IsWorkingDay
            && x.HairdresserId == hairdresserId
            && x.EffectiveFrom <= TimeOffDay, ct);
    }

    public async Task<HairdresserWorkingHours?> GetEffectiveForDayAsync(string hairdresserId, DayOfWeek dayOfWeek, DateOnly day, CancellationToken ct)
    {
        return await _context.HairdresserWorkingHours
            .AsNoTracking()
            .Where(x => x.HairdresserId == hairdresserId && x.DayOfWeek == dayOfWeek && x.EffectiveFrom <= day)
            .OrderByDescending(x => x.EffectiveFrom)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<HairdresserWorkingHours>> GetWeekAsync(string hairdresserId, DateOnly monday, CancellationToken ct)
    {
        var effectiveDatesQuery = _context.HairdresserWorkingHours
            .AsNoTracking()
            .Where(x => x.HairdresserId == hairdresserId && x.EffectiveFrom <= monday)
            .Select(x => x.EffectiveFrom);

        DateOnly? validDate = await effectiveDatesQuery
            .OrderByDescending(x => x)
            .FirstOrDefaultAsync(ct);

        if (validDate is null)
            return [];

        return await _context.HairdresserWorkingHours
            .AsNoTracking()
            .Where(x => x.HairdresserId == hairdresserId && x.EffectiveFrom == validDate.Value)
            .OrderBy(x => x.DayOfWeek)
            .ToListAsync(ct);
    }

    public async Task<List<HairdresserLookupRow>> GetActiveHairdressersWithAnyFullWeekAsync(CancellationToken ct)
    {
        var result = await (
            from u in _context.Users
            where u.IsActive
            join wh in _context.HairdresserWorkingHours on u.Id equals wh.HairdresserId
            group wh by new { wh.HairdresserId, u.FirstName, u.LastName } into g
            where g.Count() >= 5
            select new HairdresserLookupRow
            {
                Id = g.Key.HairdresserId,
                FirstName = g.Key.FirstName,
                LastName = g.Key.LastName
            }
         )
         .Distinct()
         .OrderBy(x => x.LastName)
         .ToListAsync(ct);

        return result;
    }

    public async Task<bool> ExistWorkingHoursByHairdresser(string hairdresserId, CancellationToken ct)
    {
        return await context.HairdresserWorkingHours
            .Where(x => x.HairdresserId == hairdresserId)
            .Take(5)
            .CountAsync(ct) == 5;
    }
}
