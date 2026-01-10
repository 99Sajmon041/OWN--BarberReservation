using AutoMapper;
using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Reservation.Hairdresser;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Reservation.Queries.Hairdresser.GetHairDresserReservation;

public sealed class GetHairDresserReservationQueryHandler(
    ILogger<GetHairDresserReservationQueryHandler> logger,
    IUnitOfWork unitOfWork,
    ICurrentAppUser currentAppUser,
    IMapper mapper) : IRequestHandler<GetHairDresserReservationQuery, HairdresserReservationDto>
{
    public async  Task<HairdresserReservationDto> Handle(GetHairDresserReservationQuery request, CancellationToken ct)
    {
        var reservation = await unitOfWork.ReservationRepository.GetForHairdresserAsync(request.Id, currentAppUser.User.Id, ct);
        if (reservation is null)
        {
            logger.LogWarning("Reservation with id {ReservationId} not found for hairdresser {HairdresserId}", request.Id, currentAppUser.User.Id);
            throw new NotFoundException("Reservation not found");
        }

        var reservationDto = mapper.Map<HairdresserReservationDto>(reservation);

        logger.LogInformation("Retrieved reservation with id {ReservationId} for hairdresser {HairdresserId}", request.Id, currentAppUser.User.Id);

        return reservationDto;
    }
}
