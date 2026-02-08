using AutoMapper;
using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Reservation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Reservation.Queries.Admin.GetAdminReservation;

public sealed class GetAdminReservationQueryHandler(
    ILogger<GetAdminReservationQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAdminReservationQuery, ReservationDto>
{
    public async Task<ReservationDto> Handle(GetAdminReservationQuery request, CancellationToken ct)
    {
        var reservation = await unitOfWork.ReservationRepository.GetForAdminAsync(request.Id, ct);
        if(reservation is null)
        {
            logger.LogWarning("Reservation with Id {ReservationId} not found.", request.Id);
            throw new NotFoundException("Rezervace nebyla nalezena.");
        }

        var reservationDto = mapper.Map<ReservationDto>(reservation);

        logger.LogInformation("Admin fetched reservation with Id {ReservationId}.", request.Id);

        return reservationDto;
    }
}
