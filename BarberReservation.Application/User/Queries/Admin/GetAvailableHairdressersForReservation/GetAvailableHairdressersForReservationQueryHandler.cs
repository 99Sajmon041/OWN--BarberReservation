using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.LookUpModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Queries.Admin.GetAvailableHairdressersForReservation;

public sealed class GetAvailableHairdressersForReservationQueryHandler(
    ILogger<GetAvailableHairdressersForReservationQueryHandler> logger,
    IUnitOfWork unitOfWork,
    UserManager<ApplicationUser> userManager) : IRequestHandler<GetAvailableHairdressersForReservationQuery, List<GetLookUpHairdressers>>
{
    public async Task<List<GetLookUpHairdressers>> Handle(GetAvailableHairdressersForReservationQuery request, CancellationToken ct)
    {
        var reservation = await unitOfWork.ReservationRepository.GetForAdminAsync(request.ReservationId, ct);
        if (reservation is null)
        {
            logger.LogWarning("Reservation {ReservationId} not found.", request.ReservationId);
            throw new NotFoundException("Rezervace nebyla nalezena.");
        }

        if (reservation.Status != ReservationStatus.Booked)
        {
            logger.LogWarning("Reservation {ReservationId} is not Booked. Status: {Status}", reservation.Id, reservation.Status);
            throw new ConflictException("Rezervaci lze změnit jen ve stavu 'Booked'.");
        }

        if (reservation.StartAt <= DateTime.Now)
        {
            logger.LogWarning("Reservation {ReservationId} already started. StartAt: {StartAt:u}", reservation.Id, reservation.StartAt);
            throw new ConflictException("Kadeřníka lze změnit pouze před začátkem rezervace.");
        }

        var startTime = TimeOnly.FromDateTime(reservation.StartAt);
        var endTime = TimeOnly.FromDateTime(reservation.EndAt);
        var dayOfWeek = reservation.StartAt.DayOfWeek;

        var hairdressers  = await userManager.GetUsersInRoleAsync(nameof(UserRoles.Hairdresser));
        var result = new List<GetLookUpHairdressers>();

        foreach (var hairdresser in hairdressers )
        {
            ct.ThrowIfCancellationRequested();

            var workHours = await unitOfWork.HairdresserWorkingHoursRepository.GetForDayAsync(hairdresser.Id, dayOfWeek, ct);
            if (workHours is null || !workHours.IsWorkingDay)
                continue;

            if (startTime < workHours.WorkFrom || endTime > workHours.WorkTo)
                continue;

            var timeOffOverlapExists = await unitOfWork.HairdresserTimeOffRepository.ExistsOverlapAsync(hairdresser.Id, reservation.StartAt, reservation.EndAt, ct);
            if (timeOffOverlapExists)
                continue;

            var reservationOverlapExists = await unitOfWork.ReservationRepository.ExistsOverlapForHairdresserAsync(hairdresser.Id, reservation.StartAt, reservation.EndAt, ct);
            if (reservationOverlapExists)
                continue;

            var existingHairdresserService = await unitOfWork.HairdresserServiceRepository.ExistsActiveWithSameServiceAsync(
                hairdresser.Id,
                reservation.HairdresserService.ServiceId, 
                ct);

            if (!existingHairdresserService)
                continue;

            result.Add(new GetLookUpHairdressers
            {
                Id = hairdresser.Id,
                FullName = $"{hairdresser.FirstName} {hairdresser.LastName}"
            });
        }

        logger.LogInformation(
            "Admin fetched available hairdressers for reservation {ReservationId} ({StartAt:u} - {EndAt:u}). Candidates: {Count}",
            reservation.Id,
            reservation.StartAt,
            reservation.EndAt,
            result.Count);

        return result;
    }
}
