using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Infrastructure.Database;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Reservation.Common;
using BarberReservation.Shared.Models.Reservation.Hairdresser;
using BarberReservation.Shared.Models.Reservation.Self;
using Microsoft.EntityFrameworkCore;

namespace BarberReservation.Infrastructure.Repositories;

public sealed class ReservationRepository(BarberDbContext context) : IReservationRepository
{
    private readonly BarberDbContext _context = context;

    public async Task<bool> ExistsByHairDresserServiceIdAsync(int hairDresserServiceId, CancellationToken ct)
    {
        return await _context.Reservations.AnyAsync(x => x.HairdresserServiceId == hairDresserServiceId, ct);
    }

    public async Task<Reservation?> GetForAdminAsync(int id, CancellationToken ct)
    {
        return await _context.Reservations
            .Include(x => x.Hairdresser)
            .Include(x => x.HairdresserService)
            .ThenInclude(x => x.Service)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<Reservation?> GetForHairdresserAsync(int id, string hairDresserId, CancellationToken ct)
    {
        return await _context.Reservations
            .Include(x => x.HairdresserId)
            .Include(x => x.HairdresserService)
            .ThenInclude(x => x.Service)
            .FirstOrDefaultAsync(x => x.Id == id && x.HairdresserId == hairDresserId, ct);
    }

    public async Task<Reservation?> GetForClientAsync(int id, string userId, CancellationToken ct)
    {
        return await _context.Reservations
            .Include(x => x.Hairdresser)
            .Include(x => x.HairdresserService)
            .ThenInclude(x => x.Service)
            .FirstOrDefaultAsync(x => x.Id == id && x.CustomerId == userId, ct);
    }

    public async Task CreateAsync(Reservation reservation, CancellationToken ct)
    {
        await _context.Reservations.AddAsync(reservation, ct);
    }

    public async Task<bool> ExistsOverlapForHairdresserAsync(string hairdresserId, DateTime startAt, DateTime endAt, CancellationToken ct)
    {
        return await _context.Reservations
            .AsNoTracking()
            .AnyAsync(x => x.HairdresserId == hairdresserId && x.Status != ReservationStatus.Canceled && (x.StartAt < endAt && x.EndAt > startAt), ct);
    }

    public async Task<bool> ExistAnyUpComingReservationAsync(string userId, CancellationToken ct)
    {
        return await _context.Reservations.AnyAsync(x => x.CustomerId == userId && x.Status != ReservationStatus.Canceled && x.StartAt >= DateTime.UtcNow, ct);
    }

    public async Task<(IReadOnlyList<Reservation>, int)> GetPagedForAdminAsync(AdminReservationPagedRequest request, CancellationToken ct)
    {
        var query = GetBaseQuery();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();

            query = query.Where(x =>
                x.Hairdresser.FirstName.Contains(term) ||
                x.Hairdresser.LastName.Contains(term) ||
                x.HairdresserService.Service.Name.Contains(term) ||
                x.CustomerEmail != null && x.CustomerEmail.Contains(term) ||
                x.CustomerPhone != null && x.CustomerPhone.Contains(term) ||
                x.CustomerName != null && x.CustomerName.Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(request.HairdresserId))
            query = query.Where(x => x.HairdresserId == request.HairdresserId);

        if(request.ServiceId.HasValue)
            query = query.Where(x => x.HairdresserService.ServiceId == request.ServiceId.Value);

        if (request.Status.HasValue)
            query = query.Where(x => x.Status == request.Status.Value);

        if (request.CanceledBy.HasValue)
            query = query.Where(x => x.CanceledBy == request.CanceledBy.Value);

        if (request.CanceledReason.HasValue)
            query = query.Where(x => x.CanceledReason == request.CanceledReason.Value);

        if (request.CreatedFrom.HasValue)
            query = query.Where(x => x.CreatedAt >= request.CreatedFrom.Value);

        if (request.CreatedTo.HasValue)
            query = query.Where(x => x.CreatedAt <= request.CreatedTo.Value);

        if (request.StartFrom.HasValue)
            query = query.Where(x => x.StartAt >= request.StartFrom.Value);

        if (request.StartTo.HasValue)
            query = query.Where(x => x.StartAt <= request.StartTo.Value);

        if (request.CanceledFrom.HasValue)
            query = query.Where(x => x.CanceledAt.HasValue && x.CanceledAt.Value >= request.CanceledFrom.Value);

        if (request.CanceledTo.HasValue)
            query = query.Where(x => x.CanceledAt.HasValue && x.CanceledAt.Value <= request.CanceledTo.Value);

        var total = await query.CountAsync(ct);

        var sortBy = string.IsNullOrWhiteSpace(request.SortBy) ? "id" : request.SortBy.Trim().ToLowerInvariant();

        query = sortBy switch
        {
            "hairdresser" => request.Desc
                ? query.OrderByDescending(x => x.Hairdresser.LastName).ThenByDescending(x => x.Hairdresser.FirstName)
                : query.OrderBy(x => x.Hairdresser.LastName).ThenBy(x => x.Hairdresser.FirstName),

            "service" => request.Desc
                ? query.OrderByDescending(x => x.HairdresserService.Service.Name)
                : query.OrderBy(x => x.HairdresserService.Service.Name),

            "status" => request.Desc
                ? query.OrderByDescending(x => x.Status)
                : query.OrderBy(x => x.Status),

            "createdat" => request.Desc
                ? query.OrderByDescending(x => x.CreatedAt)
                : query.OrderBy(x => x.CreatedAt),

            "startat" => request.Desc
                ? query.OrderByDescending(x => x.StartAt)
                : query.OrderBy(x => x.StartAt),

            "canceledat" => request.Desc
                ? query.OrderByDescending(x => x.CanceledAt)
                : query.OrderBy(x => x.CanceledAt),

            _ => request.Desc
                ? query.OrderByDescending(x => x.Id)
                : query.OrderBy(x => x.Id)
        };

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<(IReadOnlyList<Reservation>, int)> GetPagedForHairdresserAsync(HairdresserReservationPagedRequest request, string hairDresserId, CancellationToken ct)
    {
        var query = GetBaseQuery();

        query = query.Where(x => x.HairdresserId == hairDresserId);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();

            query = query.Where(x =>
                x.HairdresserService.Service.Name.Contains(term) ||
                x.CustomerEmail != null && x.CustomerEmail.Contains(term) ||
                x.CustomerPhone != null &&  x.CustomerPhone.Contains(term) ||
                x.CustomerName != null && x.CustomerName.Contains(term));
        }

        if (request.ServiceId.HasValue)
            query = query.Where(x => x.HairdresserService.ServiceId == request.ServiceId.Value);

        if (request.Status.HasValue)
            query = query.Where(x => x.Status == request.Status.Value);

        if (request.CanceledBy.HasValue)
            query = query.Where(x => x.CanceledBy == request.CanceledBy.Value);

        if (request.CanceledReason.HasValue)
            query = query.Where(x => x.CanceledReason == request.CanceledReason.Value);

        if (request.CreatedFrom.HasValue)
            query = query.Where(x => x.CreatedAt >= request.CreatedFrom.Value);

        if (request.CreatedTo.HasValue)
            query = query.Where(x => x.CreatedAt <= request.CreatedTo.Value);

        if (request.StartFrom.HasValue)
            query = query.Where(x => x.StartAt >= request.StartFrom.Value);

        if (request.StartTo.HasValue)
            query = query.Where(x => x.StartAt <= request.StartTo.Value);

        if (request.CanceledFrom.HasValue)
            query = query.Where(x => x.CanceledAt.HasValue && x.CanceledAt.Value >= request.CanceledFrom.Value);

        if (request.CanceledTo.HasValue)
            query = query.Where(x => x.CanceledAt.HasValue && x.CanceledAt.Value <= request.CanceledTo.Value);

        var total = await query.CountAsync(ct);

        var sortBy = string.IsNullOrWhiteSpace(request.SortBy) ? "id" : request.SortBy.Trim().ToLowerInvariant();

        query = sortBy switch
        {
            "service" => request.Desc
                ? query.OrderByDescending(x => x.HairdresserService.Service.Name)
                : query.OrderBy(x => x.HairdresserService.Service.Name),

            "status" => request.Desc
                ? query.OrderByDescending(x => x.Status)
                : query.OrderBy(x => x.Status),

            "createdat" => request.Desc
                ? query.OrderByDescending(x => x.CreatedAt)
                : query.OrderBy(x => x.CreatedAt),

            "startat" => request.Desc
                ? query.OrderByDescending(x => x.StartAt)
                : query.OrderBy(x => x.StartAt),

            "canceledat" => request.Desc
                ? query.OrderByDescending(x => x.CanceledAt)
                : query.OrderBy(x => x.CanceledAt),

            _ => request.Desc
                ? query.OrderByDescending(x => x.Id)
                : query.OrderBy(x => x.Id)
        };

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<(IReadOnlyList<Reservation>, int)> GetPagedForClientAsync(SelfReservationPagedRequest request, string userId, CancellationToken ct)
    {
        var query = GetBaseQuery();
        query = query.Where(x => x.CustomerId == userId);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();

            query = query.Where(x =>
                x.Hairdresser.FirstName.Contains(term) ||
                x.Hairdresser.LastName.Contains(term) ||
                x.HairdresserService.Service.Name.Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(request.HairdresserId))
            query = query.Where(x => x.HairdresserId == request.HairdresserId);

        if (request.ServiceId.HasValue)
            query = query.Where(x => x.HairdresserService.ServiceId == request.ServiceId.Value);

        if (request.Status.HasValue)
            query = query.Where(x => x.Status == request.Status.Value);

        if (request.CanceledBy.HasValue)
            query = query.Where(x => x.CanceledBy == request.CanceledBy.Value);

        if (request.CanceledReason.HasValue)
            query = query.Where(x => x.CanceledReason == request.CanceledReason.Value);

        if (request.CreatedFrom.HasValue)
            query = query.Where(x => x.CreatedAt >= request.CreatedFrom.Value);

        if (request.CreatedTo.HasValue)
            query = query.Where(x => x.CreatedAt <= request.CreatedTo.Value);

        if (request.StartFrom.HasValue)
            query = query.Where(x => x.StartAt >= request.StartFrom.Value);

        if (request.StartTo.HasValue)
            query = query.Where(x => x.StartAt <= request.StartTo.Value);

        if (request.CanceledFrom.HasValue)
            query = query.Where(x => x.CanceledAt.HasValue && x.CanceledAt.Value >= request.CanceledFrom.Value);

        if (request.CanceledTo.HasValue)
            query = query.Where(x => x.CanceledAt.HasValue && x.CanceledAt.Value <= request.CanceledTo.Value);

        var total = await query.CountAsync(ct);

        var sortBy = string.IsNullOrWhiteSpace(request.SortBy) ? "id" : request.SortBy.Trim().ToLowerInvariant();

        query = sortBy switch
        {
            "hairdresser" => request.Desc
                ? query.OrderByDescending(x => x.Hairdresser.LastName).ThenByDescending(x => x.Hairdresser.FirstName)
                : query.OrderBy(x => x.Hairdresser.LastName).ThenBy(x => x.Hairdresser.FirstName),

            "service" => request.Desc
                ? query.OrderByDescending(x => x.HairdresserService.Service.Name)
                : query.OrderBy(x => x.HairdresserService.Service.Name),

            "status" => request.Desc
                ? query.OrderByDescending(x => x.Status)
                : query.OrderBy(x => x.Status),

            "createdat" => request.Desc
                ? query.OrderByDescending(x => x.CreatedAt)
                : query.OrderBy(x => x.CreatedAt),

            "startat" => request.Desc
                ? query.OrderByDescending(x => x.StartAt)
                : query.OrderBy(x => x.StartAt),

            "canceledat" => request.Desc
                ? query.OrderByDescending(x => x.CanceledAt)
                : query.OrderBy(x => x.CanceledAt),

            _ => request.Desc
                ? query.OrderByDescending(x => x.Id)
                : query.OrderBy(x => x.Id)
        };

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    private IQueryable<Reservation> GetBaseQuery()
    {
        return _context.Reservations
            .AsNoTracking()
            .Include(x => x.Hairdresser)
            .Include(x => x.HairdresserService)
            .ThenInclude(x => x.Service);
    }

    public async Task<List<Reservation>> GetForHairdresserDailyAsync(string hairdresserId, DateOnly day, CancellationToken ct)
    {
        var from = day.ToDateTime(TimeOnly.MinValue);
        var to = from.AddDays(1);

        return await _context.Reservations
            .AsNoTracking()
            .Include(x => x.HairdresserService)
            .ThenInclude(x => x.Service)
            .Where(x => x.HairdresserId == hairdresserId && x.StartAt >= from && x.StartAt < to)
            .OrderBy(x => x.StartAt)
            .ToListAsync(ct);
    }
}
