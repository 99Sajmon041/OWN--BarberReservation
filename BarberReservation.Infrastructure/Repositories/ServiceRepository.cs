using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BarberReservation.Infrastructure.Repositories;

public sealed class ServiceRepository(BarberDbContext context) : IServiceRepository
{
    private readonly BarberDbContext _context = context;

    public async Task CreateAsync(Service service, CancellationToken ct)
    {
        await _context.Services.AddAsync(service, ct);
    }

    public Task DeactivateAsync(Service service, CancellationToken ct)
    {
        service.IsActive = false;
        return Task.CompletedTask;
    }

    public async Task<Service?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Services.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<(IReadOnlyList<Service>, int)> GetAllAsync(int page, int pageSize, bool? isActive, string? search, string? sortBy, bool desc, CancellationToken ct)
    {
        page = page <= 0 ? 1 : page;
        pageSize = pageSize <= 0 ? 10 : pageSize;
        search ??= string.Empty;
        sortBy ??= string.Empty;

        IQueryable<Service> query = _context.Services.AsNoTracking();

        if(isActive.HasValue)
            query = query.Where(x => x.IsActive == isActive.Value);

        if(!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(x => x.Name.Contains(term) || x.Description.Contains(term));
        }

        var total = await query.CountAsync(ct);

        sortBy = string.IsNullOrWhiteSpace(sortBy) ? "id" : sortBy.Trim().ToLowerInvariant();

        query = sortBy switch
        {
            "name" => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
            "description" => desc ? query.OrderByDescending(x => x.Description) : query.OrderBy(x => x.Description),
            "id" => desc ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
            "isactive" => desc ? query.OrderByDescending(x => x.IsActive) : query.OrderBy(x => x.IsActive),
            _ => query.OrderBy(x => x.Id)
        };

        var items = await query.
            Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }
}
