using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Reservation.Commands.Admin.UpdateReservationStatus;

public sealed class UpdateReservationStatusCommandHandler(
    ILogger<UpdateReservationStatusCommandHandler> logger,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateReservationStatusCommand>
{
    public async Task<Unit> Handle(UpdateReservationStatusCommand request, CancellationToken ct)
    {
        var reservation = await unitOfWork.ReservationRepository.GetForAdminAsync(request.Id, ct);
        if (reservation is null)
        {
            logger.LogWarning("Reservation with Id {ReservationId} not found.", request.Id);
            throw new NotFoundException("Rezervace nebyla nalezena.");
        }

        if (reservation.Status == ReservationStatus.Canceled)
        {
            logger.LogWarning(
                "Admin attempted to change reservation status but it is already Canceled. ReservationId: {ReservationId}, RequestedStatus: {RequestedStatus}.",
                request.Id,
                request.NewReservationStatus);

            throw new ForbiddenException("Zrušenou rezervaci nelze měnit.");
        }

        reservation.Status = request.NewReservationStatus;
        

        if (request.NewReservationStatus == ReservationStatus.Canceled)
        {
            reservation.CanceledBy = ReservationCanceledBy.Admin;
            reservation.CanceledAt = DateTime.UtcNow;
            reservation.CanceledReason = request.CanceledReason;
        }

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Admin changed reservation status with Reservation ID: {ReservationId} to: {NewReserVationStatus}.", request.Id, request.NewReservationStatus);

        return Unit.Value;
    }
}
