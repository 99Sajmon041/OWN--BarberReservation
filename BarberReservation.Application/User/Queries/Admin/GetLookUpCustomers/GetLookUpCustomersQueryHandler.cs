using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.LookUpModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Queries.Admin.GetLookUpCustomers;

public sealed class GetLookUpCustomersQueryHandler(
    ILogger<GetLookUpCustomersQueryHandler> logger,
    IReservationLookupsRepository reservationLookupsRepository
    ) : IRequestHandler<GetLookUpCustomersQuery, IEnumerable<ReservationClientLookUpDto>>
{
    public async Task<IEnumerable<ReservationClientLookUpDto>> Handle(GetLookUpCustomersQuery request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var term = (request.Search ?? "").Trim();

        if (term.Length < 3)
            return [];

        var result = await reservationLookupsRepository.SearchCustomersAsync(term, 20, ct);

        logger.LogInformation("Admin / Hairdresser retrieved {Count} customers for lookup. Term: '{Term}'.", result.Count, term);

        return result;
    }
}
