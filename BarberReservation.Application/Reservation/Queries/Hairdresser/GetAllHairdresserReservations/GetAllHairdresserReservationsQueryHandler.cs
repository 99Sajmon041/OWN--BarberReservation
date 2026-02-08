using AutoMapper;
using BarberReservation.Application.Reservation.Mapping;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Reservation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Reservation.Queries.Hairdresser.GetAllHairdresserReservations;

public sealed class GetAllHairdresserReservationsQueryHandler(
    ILogger<GetAllHairdresserReservationsQueryHandler> logger,
    IUnitOfWork unitOfWork,
    ICurrentAppUser currentAppUser,
    IMapper mapper) : IRequestHandler<GetAllHairdresserReservationsQuery, PagedResult<ReservationDto>>
{
    public async Task<PagedResult<ReservationDto>> Handle(GetAllHairdresserReservationsQuery request, CancellationToken ct)
    {
        var hairdresserReservationPagedRequest = request.ToHairDresserReservationPagedRequest();

        request.HairdresserId = currentAppUser.User.Id;

        var (items, total) = await unitOfWork.ReservationRepository.GetPagedForHairdresserAsync(hairdresserReservationPagedRequest, currentAppUser.User.Id, ct);

        var reservationsDto = mapper.Map<IReadOnlyList<ReservationDto>>(items);

        logger.LogInformation("Hairdresser with ID: {HairdresserId} fetched filtered {ItemsCount} of own reservations.", currentAppUser.User.Id, items.Count);

        return new PagedResult<ReservationDto>
        {
            Items = reservationsDto,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItemsCount = total
        };
    }
}