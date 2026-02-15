using AutoMapper;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Reservation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Reservation.Queries.Admin.GetAllWeeklyAdminReservations;

public sealed class GetAllWeeklyAdminReservationsQueryHandler(
    ILogger<GetAllWeeklyAdminReservationsQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllWeeklyAdminReservationsQuery, List<ReservationDto>>
{
    public async Task<List<ReservationDto>> Handle(GetAllWeeklyAdminReservationsQuery request, CancellationToken ct)
    {
        var reservations = await unitOfWork.ReservationRepository.GetAllWeeklyAsync(request.HairdresserId, request.Monday, ct);

        var reservationsDto = mapper.Map<List<ReservationDto>>(reservations);

        logger.LogInformation(
            "Admin requested weekly reservations. HairdresserId: {HairdresserId}, WeekStart: {WeekStart}, WeekEndExclusive: {WeekEndExclusive}, Records: {Count}",
            request.HairdresserId,
            request.Monday.Date,
            request.Monday.Date.AddDays(5),
            reservations.Count);

        return reservationsDto;
    }
}
