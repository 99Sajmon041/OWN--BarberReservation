using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Reservation.Commands.Admin.ChangeReservationHairdresser;

public sealed class ChangeReservationHairdresserCommandHandler(
    ILogger<ChangeReservationHairdresserCommandHandler> logger,
    IUnitOfWork unitOfWork,
    UserManager<ApplicationUser> userManager) : IRequestHandler<ChangeReservationHairdresserCommand>
{
    public async Task Handle(ChangeReservationHairdresserCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var reservation = await unitOfWork.ReservationRepository.GetForAdminAsync(request.ReservationId, ct);
        if (reservation is null)
        {
            logger.LogWarning("ChangeReservationHairdresser failed: reservation {ReservationId} not found.", request.ReservationId);
            throw new NotFoundException("Rezervace nebyla nalezena.");
        }

        if (reservation.Status != ReservationStatus.Booked)
        {
            logger.LogWarning(
                "ChangeReservationHairdresser blocked: reservation {ReservationId} is not Booked (Status: {Status}).",
                reservation.Id, reservation.Status);

            throw new ConflictException("Rezervaci lze změnit jen ve stavu 'Booked'.");
        }

        if (reservation.StartAt <= DateTime.Now)
        {
            logger.LogWarning("ChangeReservationHairdresser blocked: reservation {ReservationId} already started (StartAt: {StartAt:u}).", reservation.Id, reservation.StartAt);
            throw new ConflictException("Kadeřníka lze změnit pouze před začátkem rezervace.");
        }

        if (string.Equals(reservation.HairdresserId, request.HairdresserId, StringComparison.OrdinalIgnoreCase))
        {
            logger.LogInformation(
                "ChangeReservationHairdresser skipped: reservation {ReservationId} already has hairdresser {HairdresserId}.",
                reservation.Id, reservation.HairdresserId);

            throw new ConflictException("Jedná se o stejného kadeřníka. Změna neproběhla.");
        }

        var hairdresser = await userManager.FindByIdAsync(request.HairdresserId);
        if (hairdresser is null)
        {
            logger.LogWarning("ChangeReservationHairdresser failed: hairdresser {HairdresserId} not found.", request.HairdresserId);
            throw new NotFoundException("Kadeřník nebyl nalezen.");
        }

        var isHairdresser = await userManager.IsInRoleAsync(hairdresser, nameof(UserRoles.Hairdresser));
        if (!isHairdresser)
        {
            logger.LogWarning("ChangeReservationHairdresser failed: user {UserId} is not in role {Role}.", hairdresser.Id, nameof(UserRoles.Hairdresser));
            throw new NotFoundException("Kadeřník nebyl nalezen.");
        }

        var serviceId = reservation.HairdresserService.ServiceId;

        var newHairdresserServiceId = await unitOfWork.HairdresserServiceRepository.GetActiveHairdresserServiceIdAsync(hairdresser.Id, serviceId, ct);
        if (newHairdresserServiceId is null)
        {
            logger.LogWarning(
                "ChangeReservationHairdresser blocked: hairdresser {HairdresserId} has no active HairdresserService for ServiceId {ServiceId}.",
                hairdresser.Id, serviceId);

            throw new ConflictException("Vybraný kadeřník tuto službu neposkytuje.");
        }

        var startTime = TimeOnly.FromDateTime(reservation.StartAt);
        var endTime = TimeOnly.FromDateTime(reservation.EndAt);
        var dayOfWeek = reservation.StartAt.DayOfWeek;

        var workHours = await unitOfWork.HairdresserWorkingHoursRepository.GetForDayAsync(hairdresser.Id, dayOfWeek, ct);
        if (workHours is null || !workHours.IsWorkingDay)
        {
            logger.LogWarning("ChangeReservationHairdresser blocked: hairdresser {HairdresserId} does not work on {DayOfWeek}.", hairdresser.Id, dayOfWeek);
            throw new ConflictException("Kadeřník v tento den nepracuje.");
        }

        if (startTime < workHours.WorkFrom || endTime > workHours.WorkTo)
        {
            logger.LogWarning(
                "ChangeReservationHairdresser blocked: hairdresser {HairdresserId} working hours ({WorkFrom} - {WorkTo}) do not cover ({StartTime} - {EndTime}).",
                hairdresser.Id, workHours.WorkFrom, workHours.WorkTo, startTime, endTime);

            throw new ConflictException("Kadeřník v tento čas nepracuje.");
        }

        var timeOffOverlapExists = await unitOfWork.HairdresserTimeOffRepository.ExistsOverlapAsync(hairdresser.Id, reservation.StartAt, reservation.EndAt, ct);

        if (timeOffOverlapExists)
        {
            logger.LogWarning(
                "ChangeReservationHairdresser blocked: hairdresser {HairdresserId} has time off overlap ({StartAt:u} - {EndAt:u}).",
                hairdresser.Id, reservation.StartAt, reservation.EndAt);

            throw new ConflictException("Kadeřník má v tento čas volno.");
        }

        var reservationOverlapExists = await unitOfWork.ReservationRepository.ExistsOverlapForHairdresserAsync(hairdresser.Id, reservation.StartAt, reservation.EndAt, ct);

        if (reservationOverlapExists)
        {
            logger.LogWarning(
                "ChangeReservationHairdresser blocked: hairdresser {HairdresserId} has reservation overlap ({StartAt:u} - {EndAt:u}).",
                hairdresser.Id, reservation.StartAt, reservation.EndAt);

            throw new ConflictException("Kadeřník má v tento čas jinou rezervaci.");
        }

        var oldHairdresserId = reservation.HairdresserId;
        var oldHairdresserServiceId = reservation.HairdresserServiceId;

        reservation.HairdresserId = hairdresser.Id;
        reservation.HairdresserServiceId = newHairdresserServiceId.Value;

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation(
            "Reservation {ReservationId} hairdresser changed from {OldHairdresserId} (HS:{OldHSId}) to {NewHairdresserId} (HS:{NewHSId}). Interval: {StartAt:u}-{EndAt:u}",
            reservation.Id,
            oldHairdresserId, oldHairdresserServiceId,
            reservation.HairdresserId, reservation.HairdresserServiceId,
            reservation.StartAt, reservation.EndAt);
    }
}
