using AutoMapper;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Reservation.Hairdresser;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Reservation.Queries.Hairdresser.GetHairdresserReservationByDay;

public sealed class GetHairdresserReservationByDayQueryHandler(
    ILogger<GetHairdresserReservationByDayQueryHandler> logger,
    IUnitOfWork unitOfWork,
    ICurrentAppUser currentAppUser,
    IMapper mapper) : IRequestHandler<GetHairdresserReservationByDayQuery, List<HairdresserReservationDto>>
{
    public async Task<List<HairdresserReservationDto>> Handle(GetHairdresserReservationByDayQuery request, CancellationToken ct)
    {
        var hairdresserId = currentAppUser.User.Id;
        List<HairdresserReservationDto> hairdresserReservationDtos = new();

        var reservations = await unitOfWork.ReservationRepository.GetForHairdresserDailyAsync(hairdresserId, request.Day, ct);

        if (reservations.Count > 0)
            hairdresserReservationDtos = mapper.Map<List<HairdresserReservationDto>>(reservations);

        logger.LogInformation("Hairdresser fetched own reservations on date: {Date}. Hairdresser ID: {HairdresserId}", request.Day.ToString("dd.MM.yyyy"), hairdresserId);

        return hairdresserReservationDtos;
    }
}
