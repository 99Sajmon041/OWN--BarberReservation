using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Infrastructure.Database;
using BarberReservation.Shared.Models.Service;
using Microsoft.EntityFrameworkCore;

namespace BarberReservation.Infrastructure.Repositories;

public sealed class ServiceRepository(BarberDbContext context) : BaseRepository, IServiceRepository
{
    private readonly BarberDbContext _context = context;

    public async Task CreateAsync(Service service, CancellationToken ct)
    {
        await _context.Services.AddAsync(service, ct);
    }

    public bool Deactivate(Service service)
    {
        return TryDeactivate(service);
    }

    public async Task<Service?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Services.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<(IReadOnlyList<Service>, int)> GetAllAsync(ServicePageRequest request, CancellationToken ct)
    {
        var query = _context.Services.AsNoTracking();

        if (request.IsActive.HasValue)
            query = query.Where(x => x.IsActive == request.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(x => x.Name.Contains(term) || (x.Description != null &&  x.Description.Contains(term)));
        }

        var total = await query.CountAsync(ct);

        var sortBy = string.IsNullOrWhiteSpace(request.SortBy) ? "id" : request.SortBy.Trim().ToLowerInvariant();

        query = sortBy switch
        {
            "name" => request.Desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
            "description" => request.Desc ? query.OrderByDescending(x => x.Description) : query.OrderBy(x => x.Description),
            "id" => request.Desc ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
            "isactive" => request.Desc ? query.OrderByDescending(x => x.IsActive) : query.OrderBy(x => x.IsActive),
            _ => request.Desc ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id)
        };

        var items = await query.
            Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct)
    {
        return await _context.Services.AnyAsync(x => x.Id == id, ct);
    }

    public async Task<IReadOnlyList<Service>> GetAllLookUpAsync(CancellationToken ct)
    {
        return await _context.Services
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync(ct);
    }
}
