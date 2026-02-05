using AutoMapper;
using BarberReservation.Application.Reservation.Mapping;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Reservation.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Reservation.Queries.Self.GetAllSelfReservations;

public sealed class GetAllSelfReservationsQueryHandler(
    ILogger<GetAllSelfReservationsQueryHandler> logger,
    ICurrentAppUser currentAppUser,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllSelfReservationsQuery, PagedResult<ReservationDto>>
{
    public async Task<PagedResult<ReservationDto>> Handle(GetAllSelfReservationsQuery request, CancellationToken ct)
    {
        var selfReservationPagedRequest = request.ToSelfReservationPagedRequest();
        var userId = currentAppUser.User.Id;

        var (items, total) = await unitOfWork.ReservationRepository.GetPagedForClientAsync(selfReservationPagedRequest, userId, ct);

        var reservationsDto = mapper.Map<IReadOnlyList<ReservationDto>>(items);

        logger.LogInformation("User {UserId} fetched filtered {ItemsCount} records of their reservations.", userId, items.Count);

        return new PagedResult<ReservationDto>
        {
          Items = reservationsDto,
          Page = request.Page,
          PageSize = request.PageSize,
          TotalItemsCount = total
        };
    }
}
