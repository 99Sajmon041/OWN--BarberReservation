using AutoMapper;
using BarberReservation.Domain.Entities;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.LookUpModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Queries.Admin.GetLookUpCustomers;

public sealed class GetLookUpCustomersQueryHandler(
    ILogger<GetLookUpCustomersQueryHandler> logger,
    UserManager<ApplicationUser> userManager,
    IMapper mapper) : IRequestHandler<GetLookUpCustomersQuery, IEnumerable<ReservationClientLookUpDto>>
{
    public async Task<IEnumerable<ReservationClientLookUpDto>> Handle(GetLookUpCustomersQuery request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var term = request.Search.Trim();

        var customers = await userManager.GetUsersInRoleAsync(nameof(UserRoles.Customer));

        var filtered = customers
            .Where(u =>
                (u.FirstName.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                (u.LastName.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrWhiteSpace(u.Email) && u.Email.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrWhiteSpace(u.PhoneNumber) && u.PhoneNumber.Contains(term, StringComparison.OrdinalIgnoreCase)))
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .Take(20)
            .ToList();

        var dto = mapper.Map<List<ReservationClientLookUpDto>>(filtered);

        logger.LogInformation("Admin / Hairdresser retrieved {Count} customers for lookup. Term: '{Term}'.", dto.Count, term);

        return dto;
    }
}
