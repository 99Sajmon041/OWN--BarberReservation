using BarberReservation.Application.Common.DateHelpers;
using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Reservation.Commands.Hairdresser.CreateHairDresserReservation;

public sealed class CreateHairDresserReservationCommandHandler(
    ILogger<CreateHairDresserReservationCommandHandler> logger,
    IUnitOfWork unitOfWork,
    ICurrentAppUser currentAppUser,
    UserManager<ApplicationUser> userManager) : IRequestHandler<CreateHairDresserReservationCommand, int>
{
    public async Task<int> Handle(CreateHairDresserReservationCommand request, CancellationToken ct)
    {
        var maxReservationDate = ReservationDateHelper.GetMaxReservationDate();

        if (DateOnly.FromDateTime(request.StartAt) > maxReservationDate)
        {
            logger.LogWarning("Requested reservation date {RequestedDate} by Customer name {CustomerName} exceeds maximum allowed date.",
                request.StartAt,
                request.CustomerName);

            throw new ConflictException($"Rezervaci lze vytvořit nejpozději do data: {maxReservationDate:dd.MM.yyyy}.");
        }

        var hairDresserid = currentAppUser.User.Id;

        var hairdresserService = await unitOfWork.HairdresserServiceRepository.GetByHairdresserAndServiceAsync(hairDresserid, request.ServiceId, ct);
        if (hairdresserService is null)
        {
            logger.LogWarning("Hairdresser service with ID {ServiceId} not found or inactive.", request.ServiceId);
            throw new NotFoundException("Služba kadeřníka nenalezena.");
        }

        var startAt = request.StartAt;
        var endAt = startAt.AddMinutes(hairdresserService.DurationMinutes);

        if (endAt.Date != startAt.Date)
        {
            logger.LogWarning("Reservation crosses midnight. HairdresserId: {HairdresserId}, StartAt: {StartAt}, EndAt: {EndAt}", hairDresserid, startAt, endAt);
            throw new DomainException("Rezervace musí být v rámci jednoho dne.");
        }

        var day = startAt.DayOfWeek;
        var workingHours = await unitOfWork.HairdresserWorkingHoursRepository.GetForDayAsync(hairDresserid, day, ct);

        if (workingHours is null)
        {
            logger.LogWarning("Working hours not set. HairdresserId: {HairdresserId}, DayOfWeek: {DayOfWeek}, StartAt: {StartAt}", hairDresserid, day, startAt);
            throw new ConflictException("Kadeřník nemá pro tento den nastavenou pracovní dobu.");
        }

        var startTime = TimeOnly.FromDateTime(startAt);
        var endTime = TimeOnly.FromDateTime(endAt);

        if (startTime < workingHours.WorkFrom || endTime > workingHours.WorkTo)
        {
            logger.LogWarning(
                "Reservation is outside working hours. HairdresserId: {HairdresserId}, DayOfWeek: {DayOfWeek}, Requested: {StartTime}-{EndTime}, WorkingHours: {WorkFrom}-{WorkTo}",
                    hairDresserid, day, startTime, endTime, workingHours.WorkFrom, workingHours.WorkTo);

            throw new DomainException("Rezervace je mimo pracovní dobu kadeřníka.");
        }

        var hasTimeOff = await unitOfWork.HairdresserTimeOffRepository.ExistsOverlapAsync(hairDresserid, startAt, endAt, ct);

        if (hasTimeOff)
        {
            logger.LogWarning("Reservation overlaps hairdresser time off. HairdresserId: {HairdresserId}, StartAt: {StartAt}, EndAt: {EndAt}",
                hairDresserid, startAt, endAt);

            throw new ConflictException("Kadeřník má v tomto čase volno.");
        }

        var existsAnyReservation = await unitOfWork.ReservationRepository.ExistsOverlapForHairdresserAsync(hairDresserid, startAt, endAt, ct);
        if (existsAnyReservation)
        {
            logger.LogWarning("Reservation time slot already booked. HairdresserId: {HairdresserId}, StartAt: {StartAt}, EndAt: {EndAt}", hairDresserid, startAt, endAt);
            throw new ConflictException("V tomto čase již existuje rezervace.");
        }

        var customerId = request.CustomerId;
        var customerName = request.CustomerName;
        var customerEmail = request.CustomerEmail;
        var customerPhone = request.CustomerPhone;

        if (!string.IsNullOrWhiteSpace(request.CustomerId))
        {
            var clientById = await userManager.FindByIdAsync(request.CustomerId);
            if (clientById is null)
            {
                logger.LogInformation(
                    "CustomerId {CustomerId} not found. Using snapshot data from request. CustomerEmail: {CustomerEmail}.",
                    request.CustomerId,
                    request.CustomerEmail);
            }
            else
            {
                customerId = clientById.Id;
                customerName = clientById.FullName ?? customerName;
                customerEmail = clientById.Email ?? customerEmail;
                customerPhone = clientById.PhoneNumber ?? customerPhone;
            }
        }

        var reservation = new BarberReservation.Domain.Entities.Reservation
        {
            HairdresserId = hairDresserid,
            HairdresserServiceId = hairdresserService.Id,
            StartAt = request.StartAt,
            EndAt = endAt,
            Status = ReservationStatus.Booked,
            CreatedAt = DateTime.UtcNow,
            CustomerId = customerId,
            CustomerName = customerName,
            CustomerEmail = customerEmail,
            CustomerPhone = customerPhone
        };

        await unitOfWork.ReservationRepository.CreateAsync(reservation, ct);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation(
            "Reservation with ID {ReservationId} created by hairdresser. HairdresserId: {HairdresserId}, CustomerName: {CustomerName}, StartAt: {StartAt}.",
            reservation.Id,
            hairDresserid,
            reservation.CustomerName,
            reservation.StartAt);

        return reservation.Id;
    }
}
