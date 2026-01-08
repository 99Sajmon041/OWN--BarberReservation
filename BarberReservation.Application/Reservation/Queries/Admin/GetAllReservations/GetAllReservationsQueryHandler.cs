using AutoMapper;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Rezervation.Admin;
using MediatR;
using Microsoft.Extensions.Logging;
using BarberReservation.Shared.Models.Rezervation.Common;

namespace BarberReservation.Application.Reservation.Queries.Admin.GetAllReservations;

public sealed class GetAllReservationsQueryHandler(
    ILogger<GetAllReservationsQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllReservationsQuery, PagedResult<AdminReservationDto>>
{
    public async Task<PagedResult<AdminReservationDto>> Handle(GetAllReservationsQuery request, CancellationToken ct)
    {
        var (items, total) = await unitOfWork.ReservationRepository.GetPagedForAdminAsync(new AdminReservationPagedRequest
        {
            Page = request.Page,
            PageSize = request.PageSize,
            HairdresserId = request.HairdresserId,
            Status = request.Status,
            CanceledBy= request.CanceledBy,
            CanceledReason= request.CanceledReason,
            CreatedFrom = request.CreatedFrom,
            CreatedTo = request.CreatedTo,
            StartFrom = request.StartFrom,
            StartTo = request.StartTo,
            CanceledFrom = request.CanceledFrom,
            CanceledTo = request.CanceledTo,
            Search = request.Search,
            SortBy = request.SortBy,
            Desc = request.Desc
        },
        ct);

        var reservationsDto = mapper.Map<IReadOnlyList<AdminReservationDto>>(items);

        logger.LogInformation("Admin fetched filtered {ItemsCount} records of reservations.", items.Count);

        return new PagedResult<AdminReservationDto>
        {
          Items = reservationsDto,
          Page = request.Page,
          PageSize = request.PageSize,
          TotalItemsCount = total
        };
    }
}
