using AutoMapper;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Reservation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Reservation.Queries.Hairdresser.GetAllWeeklyHairdressersReservations;

public sealed class GetAllWeeklyHairdressersReservationsQueryHandler(
    ILogger<GetAllWeeklyHairdressersReservationsQueryHandler> logger,
    ICurrentAppUser currentAppUser,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllWeeklyHairdressersReservationsQuery, List<ReservationDto>>
{
    public async Task<List<ReservationDto>> Handle(GetAllWeeklyHairdressersReservationsQuery request, CancellationToken ct)
    {
        var hairdresserId = currentAppUser.User.Id;

        var reservations = await unitOfWork.ReservationRepository.GetAllWeeklyAsync(hairdresserId, request.Monday, ct);

        var reservationsDto = mapper.Map<List<ReservationDto>>(reservations);

        logger.LogInformation(
            "Hairdresser requested weekly reservations. HairdresserId: {HairdresserId}, WeekStart: {WeekStart}, WeekEndExclusive: {WeekEndExclusive}, Records: {Count}",
            hairdresserId,
            request.Monday.Date,
            request.Monday.Date.AddDays(5),
            reservations.Count);

        return reservationsDto;
    }
}
