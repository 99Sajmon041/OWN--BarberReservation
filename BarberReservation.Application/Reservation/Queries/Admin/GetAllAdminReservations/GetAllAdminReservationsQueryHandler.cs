using AutoMapper;
using BarberReservation.Application.Reservation.Mapping;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Reservation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Reservation.Queries.Admin.GetAllAdminReservations;

public sealed class GetAllAdminReservationsQueryHandler(
    ILogger<GetAllAdminReservationsQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllAdminReservationsQuery, PagedResult<ReservationDto>>
{
    public async Task<PagedResult<ReservationDto>> Handle(GetAllAdminReservationsQuery request, CancellationToken ct)
    {
        var adminReservationPagedRequest = request.ToAdminReservationPagedRequest();

        var (items, total) = await unitOfWork.ReservationRepository.GetPagedForAdminAsync(adminReservationPagedRequest, ct);

        var reservationsDto = mapper.Map<IReadOnlyList<ReservationDto>>(items);

        logger.LogInformation("Admin fetched filtered {ItemsCount} records of reservations.", items.Count);

        return new PagedResult<ReservationDto>
        {
          Items = reservationsDto,
          Page = request.Page,
          PageSize = request.PageSize,
          TotalItemsCount = total
        };
    }
}
