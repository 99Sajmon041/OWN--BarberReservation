using AutoMapper;
using BarberReservation.Application.Reservation.Mapping;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Reservation.Self;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Reservation.Queries.Self.GetAllSelfReservations;

public sealed class GetAllSelfReservationsQueryHandler(
    ILogger<GetAllSelfReservationsQueryHandler> logger,
    ICurrentAppUser currentAppUser,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllSelfReservationsQuery, PagedResult<SelfReservationDto>>
{
    public async Task<PagedResult<SelfReservationDto>> Handle(GetAllSelfReservationsQuery request, CancellationToken ct)
    {
        var selfReservationPagedRequest = request.ToSelfReservationPagedRequest();
        var userId = currentAppUser.User.Id;

        var (items, total) = await unitOfWork.ReservationRepository.GetPagedForClientAsync(selfReservationPagedRequest, userId, ct);

        var reservationsDto = mapper.Map<IReadOnlyList<SelfReservationDto>>(items);

        logger.LogInformation("User {UserId} fetched filtered {ItemsCount} records of their reservations.", userId, items.Count);

        return new PagedResult<SelfReservationDto>
        {
          Items = reservationsDto,
          Page = request.Page,
          PageSize = request.PageSize,
          TotalItemsCount = total
        };
    }
}
