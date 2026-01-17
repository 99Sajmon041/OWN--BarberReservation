using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Infrastructure.Database;
using BarberReservation.Shared.Models.TimeOff.Admin;
using BarberReservation.Shared.Models.TimeOff.Hairdresser;
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

    public async Task<IReadOnlyList<HairdresserTimeOff>> GetTimeOffFromDateAsync(string hairdresserId, DateOnly fromDate, CancellationToken ct)
    {
        return await _context.HairdresserTimeOffs
            .Where(x => x.HairdresserId == hairdresserId && x.StartAt >=  fromDate.ToDateTime(TimeOnly.MinValue))
            .ToListAsync(ct);
    }

    public void Add(HairdresserTimeOff timeOff)
    {
        _context.Add(timeOff);
    }

    public async Task<HairdresserTimeOff?> GetByIdAsync(int id, string hairdresserId, CancellationToken ct)
    {
        return await _context.HairdresserTimeOffs.FirstOrDefaultAsync(x => x.Id == id && x.HairdresserId == hairdresserId, ct);
    }

    public async Task<(IReadOnlyList<HairdresserTimeOff>, int)> GetAllPagedForAdminAsync(AdminHairdresserPagedRequest request, int? year, int? month, CancellationToken ct)
    {
        var query = _context.HairdresserTimeOffs
            .AsNoTracking()
            .Include(x => x.Hairdresser)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.HairdresserId))
            query = query.Where(x => x.HairdresserId == request.HairdresserId);

        var term = request.Search?.Trim().ToLowerInvariant();
        
        if(!string.IsNullOrWhiteSpace(term))
        {
            query = query.Where(x => x.Reason.Contains(term) || x.Hairdresser.FirstName.Contains(term) || x.Hairdresser.LastName.Contains(term));
        }

        if (year is not null && month is not null)
            query = query.Where(x => x.StartAt.Year == year && x.StartAt.Month == month);
        else if (year is not null)
            query = query.Where(x => x.StartAt.Year == year);

        var total = await query.CountAsync(ct);

        var sortBy = string.IsNullOrWhiteSpace(request.SortBy) ? "startdate" : request.SortBy.Trim();

        if(sortBy == "hairdresser")
        {
            if (request.Desc)
                query = query.OrderByDescending(x => x.Hairdresser.LastName).ThenByDescending(x => x.Hairdresser.FirstName);
            else
                query = query.OrderBy(x => x.Hairdresser.LastName).ThenBy(x => x.Hairdresser.FirstName);
        }
        else
        {
            if (request.Desc)
                query = query.OrderByDescending(x => x.StartAt);
            else
                query = query.OrderBy(x => x.StartAt);
        }

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<(IReadOnlyList<HairdresserTimeOff>, int)> GetAllPagedForHairdresserAsync(
        string hairdresserId,
        HairdresserPagedRequest request,
        int? year,
        int? month,
        CancellationToken ct)
    {
        var query = _context.HairdresserTimeOffs
            .AsNoTracking()
            .Where(x => x.HairdresserId == hairdresserId);

        var term = request.Search?.Trim().ToLowerInvariant();

        if (!string.IsNullOrWhiteSpace(term))
        {
            query = query.Where(x => x.Reason.Contains(term));
        }

        if (year is not null && month is not null)
            query = query.Where(x => x.StartAt.Year == year && x.StartAt.Month == month);
        else if (year is not null)
            query = query.Where(x => x.StartAt.Year == year);

        var total = await query.CountAsync(ct);

        if (request.Desc)
        {
            query = query.OrderByDescending(x => x.StartAt);
        }
        else
        {
            query = query.OrderBy(x => x.StartAt);
        }

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<IReadOnlyList<HairdresserTimeOff>> GetAllByDayForHairdresserAsync(string hairdresserId, DateTime today, CancellationToken ct)
    {
        return await _context
            .HairdresserTimeOffs
            .Where(x => x.HairdresserId == hairdresserId && x.StartAt.Date == today.Date)
            .ToListAsync(ct);
    }

    public void Delete(HairdresserTimeOff hairdresserTimeOff)
    {
        _context.HairdresserTimeOffs.Remove(hairdresserTimeOff);
    }
}