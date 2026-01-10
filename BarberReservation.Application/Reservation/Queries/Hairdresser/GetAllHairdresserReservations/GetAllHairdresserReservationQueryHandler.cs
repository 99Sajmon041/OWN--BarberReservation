using AutoMapper;
using BarberReservation.Application.Reservation.Mapping;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Reservation.Hairdresser;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Reservation.Queries.Hairdresser.GetAllHairdresserReservations;

public sealed class GetAllHairdresserReservationQueryHandler(
    ILogger<GetAllHairdresserReservationQueryHandler> logger,
    IUnitOfWork unitOfWork,
    ICurrentAppUser currentAppUser,
    IMapper mapper) : IRequestHandler<GetAllHairdresserReservationsQuery, PagedResult<HairdresserReservationDto>>
{
    public async Task<PagedResult<HairdresserReservationDto>> Handle(GetAllHairdresserReservationsQuery request, CancellationToken ct)
    {
        var hairdresserReservationPagedRequest = request.ToHairDresserReservationPagedRequest();

        var (items, total) = await unitOfWork.ReservationRepository.GetPagedForHairdresserAsync(hairdresserReservationPagedRequest, currentAppUser.User.Id, ct);

        var reservationsDto = mapper.Map<IReadOnlyList<HairdresserReservationDto>>(items);

        logger.LogInformation("Hairdresser with ID: {HairdresserId} fetched filtered {ItemsCount} of own reservations.", currentAppUser.User.Id, items.Count);

        return new PagedResult<HairdresserReservationDto>
        {
            Items = reservationsDto,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItemsCount = total
        };
    }
}