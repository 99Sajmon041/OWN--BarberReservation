using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Reservation.Commands.Self.SelfCancelReservation;

public sealed class SelfCancelReservationCommandHandler(
    ILogger<SelfCancelReservationCommandHandler> logger,
    ICurrentAppUser currentAppUser,
    IUnitOfWork unitOfWork) : IRequestHandler<SelfCancelReservationStatusCommand>
{
    public async Task<Unit> Handle(SelfCancelReservationStatusCommand request, CancellationToken ct)
    {
        var reservation = await unitOfWork.ReservationRepository.GetForClientAsync(request.Id, currentAppUser.User.Id, ct);
        if(reservation is null)
        {
            logger.LogWarning("Reservation with id {ReservationId} for user {UserId} not found.", request.Id, currentAppUser.User.Id);
            throw new NotFoundException("Rezervace nebyla nalezena.");
        }

        var timeUntilReservation = reservation.StartAt - DateTime.UtcNow;

        if (timeUntilReservation <= TimeSpan.Zero)
        {
            logger.LogWarning("Reservation with id {ReservationId} for user {UserId} cannot be canceled because it has already started or is in the past.",
                request.Id,
                currentAppUser.User.Id);

            throw new DomainException("Tuto rezervaci už nelze zrušit, protože už začala nebo je v minulosti.");
        }

        if (timeUntilReservation < TimeSpan.FromHours(24))
        {
            logger.LogWarning("Reservation with id {ReservationId} for user {UserId} cannot be canceled because it is less than 24 hours away.",
                request.Id,
                currentAppUser.User.Id);

            throw new DomainException("Tuto rezervaci nelze zrušit méně než 24 hodin předem.");
        }

        if (reservation.Status is ReservationStatus.Canceled or ReservationStatus.NoShow or ReservationStatus.Paid)
        {
            logger.LogWarning("Reservation with id {ReservationId} for user {UserId} cannot be canceled because its status is '{Status}'.",
                request.Id,
                currentAppUser.User.Id,
                reservation.Status);

            throw new DomainException("Tuto rezervaci nelze zrušit.");
        }


        reservation.Status = ReservationStatus.Canceled;
        reservation.CanceledAt = DateTime.UtcNow;
        reservation.CanceledBy = ReservationCanceledBy.Customer;
        reservation.CanceledReason = CanceledReason.CustomerRequest;

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Reservation with id {ReservationId} for user {UserId} has been canceled by the customer.", request.Id, currentAppUser.User.Id);

        return Unit.Value;
    }
}
