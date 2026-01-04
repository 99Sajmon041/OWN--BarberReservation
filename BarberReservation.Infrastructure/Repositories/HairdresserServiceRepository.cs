using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BarberReservation.Infrastructure.Repositories;

public sealed class HairdresserServiceRepository(BarberDbContext context) : BaseRepository, IHairdresserServiceRepository
{
    private readonly BarberDbContext _context = context;

    public async Task<bool> ExistsByServiceIdAsync(int serviceId, CancellationToken ct)
    {
        return await _context.HairdresserServices
            .AsNoTracking()
            .AnyAsync(x => x.ServiceId == serviceId && x.IsActive, ct);
    }

    public void Delete(HairdresserService hairdresserService)
    {
        _context.HairdresserServices.Remove(hairdresserService);
    }

    public async Task<HairdresserService?> GetByIdForAdminAsync(int id, CancellationToken ct)
    {
        return await _context.HairdresserServices
            .Include(x => x.Hairdresser)
            .Include(x => x.Service)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<(IReadOnlyList<HairdresserService>, int)> GetAllPagedForAdminAsync(
        int page,
        int pageSize,
        string? hairdresserId,
        int? serviceId,
        string? search,
        string? sortBy, 
        bool desc,
        bool? isActive, 
        CancellationToken ct)
    {
        search ??= string.Empty;

        var query = _context.HairdresserServices
            .Include(x => x.Hairdresser)
            .Include(x => x.Service)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(x => x.Service.Name.Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(hairdresserId))
            query = query.Where(x => x.HairdresserId == hairdresserId);

        if (serviceId is not null)
            query = query.Where(x => x.ServiceId == serviceId);

        if (isActive.HasValue)
            query = query.Where(x => x.IsActive == isActive.Value);
        else
            query = query.Where(x => x.IsActive == true);

        var total = await query.CountAsync(ct);

        sortBy = string.IsNullOrWhiteSpace(sortBy) ? "price" : sortBy.Trim().ToLowerInvariant();

        query = sortBy switch
        {
            "isactive" => desc ? query.OrderByDescending(x => x.IsActive) : query.OrderBy(x => x.IsActive),
            "durationminutes" => desc ? query.OrderByDescending(x => x.DurationMinutes): query.OrderBy(x => x.DurationMinutes),
            "servicename" => desc ? query.OrderByDescending(x => x.Service.Name) : query.OrderBy(x => x.Service.Name),
            "hairdresserlastname" => desc ? query.OrderByDescending(x => x.Hairdresser.LastName) : query.OrderBy(x => x.Hairdresser.LastName),
            _ => desc ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price)
        };

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public bool Deactivate(HairdresserService hairdresserService)
    {
        return TryDeactivate(hairdresserService);
    }

    public async Task<(IReadOnlyList<HairdresserService>, int)> GetAllPagedForCurrentUserAsync(
        int page,
        int pageSize,
        string userId,
        int? serviceId,
        string? search,
        string? sortBy,
        bool desc,
        bool? isActive,
        CancellationToken ct)
    {
        search ??= string.Empty;

        var query = _context.HairdresserServices
            .Include(x => x.Service)
            .Include(x => x.Hairdresser)
            .Where(x => x.HairdresserId == userId)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(x => x.Service.Name.Contains(term));
        }

        if (serviceId is not null)
            query = query.Where(x => x.ServiceId == serviceId);

        if (isActive.HasValue)
            query = query.Where(x => x.IsActive == isActive.Value);
        else
            query = query.Where(x => x.IsActive == true);

        var total = await query.CountAsync(ct);

        sortBy = string.IsNullOrWhiteSpace(sortBy) ? "price" : sortBy.Trim().ToLowerInvariant();

        query = sortBy switch
        {
            "isactive" => desc ? query.OrderByDescending(x => x.IsActive) : query.OrderBy(x => x.IsActive),
            "durationminutes" => desc ? query.OrderByDescending(x => x.DurationMinutes) : query.OrderBy(x => x.DurationMinutes),
            "servicename" => desc ? query.OrderByDescending(x => x.Service.Name) : query.OrderBy(x => x.Service.Name),
            _ => desc ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price)
        };

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }
}
