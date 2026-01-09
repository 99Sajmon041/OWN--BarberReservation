using AutoMapper;
using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Rezervation.Admin;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Reservation.Queries.Admin.GetReservation;

public sealed class GetReservationQueryHandler(
    ILogger<GetReservationQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetReservationQuery, AdminReservationDto>
{
    public async Task<AdminReservationDto> Handle(GetReservationQuery request, CancellationToken ct)
    {
        var reservation = await unitOfWork.ReservationRepository.GetForAdminAsync(request.Id, ct);
        if(reservation is null)
        {
            logger.LogWarning("Reservation with Id {ReservationId} not found.", request.Id);
            throw new NotFoundException("Rezervace nebyla nalezena.");
        }

        var reservationDto = mapper.Map<AdminReservationDto>(reservation);

        logger.LogInformation("Admin fetched reservation with Id {ReservationId}.", request.Id);

        return reservationDto;
    }
}
