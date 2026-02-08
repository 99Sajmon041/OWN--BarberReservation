using BarberReservation.Domain.Interfaces;
using BarberReservation.Infrastructure.Database;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.LookUpModels;
using Microsoft.EntityFrameworkCore;

namespace BarberReservation.Infrastructure.Persistence.Repositories;

public sealed class ReservationLookupsRepository(BarberDbContext db) : IReservationLookupsRepository
{
    public async Task<IReadOnlyList<ReservationClientLookUpDto>> SearchCustomersAsync(string term, int take, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var customerRoleId = await db.Roles
            .Where(r => r.Name == nameof(UserRoles.Customer))
            .Select(r => r.Id)
            .SingleAsync(ct);

        term = term.Trim();
        var like = $"%{term}%";

        return await (
            from u in db.Users.AsNoTracking()
            join ur in db.UserRoles.AsNoTracking() on u.Id equals ur.UserId
            where ur.RoleId == customerRoleId
            where
                EF.Functions.Like(u.FirstName ?? string.Empty, like) ||
                EF.Functions.Like(u.LastName ?? string.Empty, like) ||
                EF.Functions.Like(u.Email ?? string.Empty, like) ||
                EF.Functions.Like(u.PhoneNumber ?? string.Empty, like)
            orderby u.LastName, u.FirstName
            select new ReservationClientLookUpDto
            {
                CustomerId = u.Id,
                CustomerName = ((u.FirstName ?? "") + " " + (u.LastName ?? "")).Trim(),
                CustomerEmail = u.Email ?? string.Empty,
                CustomerPhone = u.PhoneNumber ?? string.Empty
            }
        )
        .Take(take)
        .ToListAsync(ct);
    }
}
