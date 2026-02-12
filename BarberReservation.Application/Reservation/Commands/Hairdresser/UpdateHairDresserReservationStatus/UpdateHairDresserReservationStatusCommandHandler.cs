using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Reservation.Commands.Hairdresser.UpdateHairDresserReservationStatus;

public sealed class UpdateHairDresserReservationStatusCommandHandler(
    ILogger<UpdateHairDresserReservationStatusCommandHandler> logger,
    IUnitOfWork unitOfWork,
    ICurrentAppUser currentAppUser) : IRequestHandler<UpdateHairDresserReservationStatusCommand>
{
    public async Task Handle(UpdateHairDresserReservationStatusCommand request, CancellationToken ct)
    {
        var now = DateTime.Now;

        var reservation = await unitOfWork.ReservationRepository.GetForHairdresserAsync(request.Id, currentAppUser.User.Id, ct);
        if(reservation is null)
        {
            logger.LogWarning("Reservation with id {ReservationId} for hairdresser {HairDresserId} not found", request.Id, currentAppUser.User.Id);
            throw new NotFoundException("Rezervace nebyla nalezena.");
        }

        if(reservation.Status is ReservationStatus.Canceled)
        {
            logger.LogWarning("Hairdresser attempted to change reservation status but it is already Canceled. ReservationId: {ReservationId}, RequestedStatus:{RequestedStatus}.",
                request.Id,
                request.NewReservationStatus);

            throw new DomainException("Rezervaci ve stavu zrušeno nelze měnit.");
        }

        if (reservation.StartAt > now && (request.NewReservationStatus == ReservationStatus.NoShow || request.NewReservationStatus == ReservationStatus.Paid))
        {
            logger.LogWarning(
                "Hairdresser attempted to set invalid status for future reservation. ReservationId: {ReservationId}, CurrentStatus: {CurrentStatus}, RequestedStatus: {RequestedStatus}, StartAt: {StartAt}, NowUtc: {NowUtc}.",
                reservation.Id,
                reservation.Status,
                request.NewReservationStatus,
                reservation.StartAt,
                now);

            throw new ConflictException("Rezervaci v budoucnu nelze nastavit na zaplaceno / zákazník se neukázal.");
        }

        reservation.Status = request.NewReservationStatus;

        if (request.NewReservationStatus == ReservationStatus.Canceled)
        {
            reservation.CanceledBy = ReservationCanceledBy.Hairdresser;
            reservation.CanceledAt = now;
            reservation.CanceledReason = request.CanceledReason;
        }
        else
        {
            reservation.CanceledBy = null;
            reservation.CanceledAt = null;
            reservation.CanceledReason = null;
        }

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Hairdresser with Id {HairDresserId} changed reservation status with Reservation ID: {ReservationId} to: {NewReserVationStatus}.",
            currentAppUser.User.Id,
            request.Id,
            request.NewReservationStatus);
    }
}
