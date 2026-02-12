using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Infrastructure.Database;
using BarberReservation.Shared.Models.HairdresserService;
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
    public async Task<IReadOnlyList<string>> GetActiveHairdresserIdsByServiceIdAsync(int serviceId, CancellationToken ct)
    {
        return await _context.HairdresserServices
            .AsNoTracking()
            .Where(x => x.ServiceId == serviceId && x.IsActive)
            .Select(x => x.HairdresserId)
            .Distinct()
            .ToListAsync(ct);
    }

    public async Task<HairdresserService?> GetByIdForCurrentUserAsync(int id, string hairdresserId, CancellationToken ct)
    {
        return await _context
            .HairdresserServices
            .Include(x => x.Service)
            .Include(x => x.Hairdresser)
            .SingleOrDefaultAsync(x => x.Id == id && x.HairdresserId == hairdresserId, ct);
    }

    public async Task<HairdresserService?> GetByIdForClientAsync(int id, string hairdresserId, CancellationToken ct)
    {
        return await _context
            .HairdresserServices
            .Include(x => x.Service)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id && x.HairdresserId == hairdresserId, ct);
    }

    public bool Deactivate(HairdresserService hairdresserService)
    {
        return TryDeactivate(hairdresserService);
    }

    public async Task CreateAsync(HairdresserService hairdresserService, CancellationToken ct)
    {
        await _context.HairdresserServices.AddAsync(hairdresserService, ct);
    }
    public async Task<bool> ExistsActiveWithSameServiceAsync(string hairdresserId, int serviceId, CancellationToken ct)
    {
        return await _context.HairdresserServices.AnyAsync(x => x.HairdresserId == hairdresserId && x.ServiceId == serviceId && x.IsActive, ct);
    }
    public async Task<(IReadOnlyList<HairdresserService>, int)> GetAllPagedForAdminAsync(CommonHairdresserServicePagedRequest request, CancellationToken ct)
    {
        var query = _context.HairdresserServices
            .Include(x => x.Hairdresser)
            .Include(x => x.Service)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => x.Service.Name.Contains(term) || (x.Hairdresser.FirstName.Contains(term) || x.Hairdresser.LastName.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(request.HairdresserId))
            query = query.Where(x => x.HairdresserId == request.HairdresserId);

        if (request.ServiceId is not null)
            query = query.Where(x => x.ServiceId == request.ServiceId);

        if (request.IsActive.HasValue)
            query = query.Where(x => x.IsActive == request.IsActive.Value);

        var total = await query.CountAsync(ct);

        var sortBy = string.IsNullOrWhiteSpace(request.SortBy) ? "price" : request.SortBy.Trim().ToLowerInvariant();

        query = sortBy switch
        {
            "isactive" => request.Desc ? query.OrderByDescending(x => x.IsActive) : query.OrderBy(x => x.IsActive),
            "durationminutes" => request.Desc ? query.OrderByDescending(x => x.DurationMinutes): query.OrderBy(x => x.DurationMinutes),
            "servicename" => request.Desc ? query.OrderByDescending(x => x.Service.Name) : query.OrderBy(x => x.Service.Name),
            "hairdresserlastname" => request.Desc ? query.OrderByDescending(x => x.Hairdresser.LastName) : query.OrderBy(x => x.Hairdresser.LastName),
            _ => request.Desc ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price)
        };

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<(IReadOnlyList<HairdresserService>, int)> GetAllPagedForCurrentUserAsync(CommonHairdresserServicePagedRequest request, string userId, CancellationToken ct)
    {
        var query = _context.HairdresserServices
            .Include(x => x.Service)
            .Include(x => x.Hairdresser)
            .Where(x => x.HairdresserId == userId)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => x.Service.Name.Contains(term));
        }

        if (request.ServiceId is not null)
            query = query.Where(x => x.ServiceId == request.ServiceId);

        if (request.IsActive.HasValue)
            query = query.Where(x => x.IsActive == request.IsActive.Value);

        var total = await query.CountAsync(ct);

        var sortBy = string.IsNullOrWhiteSpace(request.SortBy) ? "price" : request.SortBy.Trim().ToLowerInvariant();

        query = sortBy switch
        {
            "isactive" => request.Desc ? query.OrderByDescending(x => x.IsActive) : query.OrderBy(x => x.IsActive),
            "durationminutes" => request.Desc ? query.OrderByDescending(x => x.DurationMinutes) : query.OrderBy(x => x.DurationMinutes),
            "servicename" => request.Desc ? query.OrderByDescending(x => x.Service.Name) : query.OrderBy(x => x.Service.Name),
            _ => request.Desc ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price)
        };

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<bool> ExistsAnyByHairdresserAsync(string hairdresserId, CancellationToken ct)
    {
        return await _context.HairdresserServices
            .AsNoTracking()
            .AnyAsync(x => x.HairdresserId == hairdresserId && x.IsActive, ct);
    }

    public async Task<int?> GetActiveHairdresserServiceIdAsync(string hairdresserId, int serviceId, CancellationToken ct)
    {
        return await _context.HairdresserServices
            .Where(x => x.HairdresserId == hairdresserId && x.ServiceId == serviceId && x.IsActive)
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<int> GetTimeDurationByHairdresserIdAndServiceIdAsync(int serviceId, string hairdresserId, CancellationToken ct)
    {
        var hairdresserService =  await _context.HairdresserServices
            .FirstOrDefaultAsync(x => x.HairdresserId == hairdresserId && x.ServiceId == serviceId, ct);

        return hairdresserService?.DurationMinutes ?? 0;
    }

    public async Task<HairdresserService?> GetByHairdresserAndServiceAsync(string hairdresserId, int serviceId, CancellationToken ct)
    {
        return await _context.HairdresserServices
            .AsNoTracking()
            .Include(x => x.Service)
            .FirstOrDefaultAsync(x => x.HairdresserId == hairdresserId && x.ServiceId == serviceId && x.IsActive, ct);
    }
}
