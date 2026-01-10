using AutoMapper;
using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Reservation.Self;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Reservation.Queries.Self.GetSelfReservation;

public sealed class GetSelfReservationQueryHandler(
    ILogger<GetSelfReservationQueryHandler> logger,
    ICurrentAppUser currentAppUser,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetSelfReservationQuery, SelfReservationDto>
{
    public async Task<SelfReservationDto> Handle(GetSelfReservationQuery request, CancellationToken ct)
    {
        var userId = currentAppUser.User.Id;

        var reservation = await unitOfWork.ReservationRepository.GetForClientAsync(request.Id, userId, ct);
        if(reservation is null)
        {
            logger.LogWarning("Reservation with id {ReservationId} for user {UserId} not found.", request.Id, userId);
            throw new NotFoundException("Rezervace nebyla nalezena.");
        }   

        var reservationDto = mapper.Map<SelfReservationDto>(reservation);

        logger.LogInformation("Reservation with id {ReservationId} for user {UserId} retrieved successfully.", request.Id, userId);

        return reservationDto;
    }
}
