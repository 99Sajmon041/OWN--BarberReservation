using BarberReservation.Application.Common.DateHelpers;
using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Reservation.Commands.Self.CreateSelfReservation;

public sealed class CreateSelfReservationCommandHandler(
    ILogger<CreateSelfReservationCommandHandler> logger,
    IUnitOfWork unitOfWork,
    ICurrentAppUser currentAppUser,
    UserManager<ApplicationUser> userManager,
    IEmailService emailService) : IRequestHandler<CreateSelfReservationCommand>
{
    public async Task Handle(CreateSelfReservationCommand request, CancellationToken ct)
    {
        var maxReservationDate = ReservationDateHelper.GetMaxReservationDate();

        if (DateOnly.FromDateTime(request.StartAt )> maxReservationDate)
        {
            logger.LogWarning("Requested reservation date {RequestedDate} by Customer name {CustomerName} exceeds maximum allowed date.",
                request.StartAt,
                request.CustomerName);

            throw new ConflictException($"Rezervaci lze vytvořit nejpozději do data: {maxReservationDate:dd.MM.yyyy}.");
        }

        var user = await userManager.FindByIdAsync(request.HairdresserId);
        if (user is null)
        {
            logger.LogWarning("Hairdresser with ID {HairdresserId} not found.", request.HairdresserId);
            throw new NotFoundException("Uživatel - kadeřník nenalezen.");
        }

        var isHairdresser = await userManager.IsInRoleAsync(user, nameof(UserRoles.Hairdresser));
        if (!isHairdresser)
        {
            logger.LogWarning("User with ID {UserId} is not a hairdresser.", user.Id);
            throw new NotFoundException("Kadeřník nenalezen.");
        }

        var hairdresserService = await unitOfWork.HairdresserServiceRepository.GetByHairdresserAndServiceAsync(request.HairdresserId, request.ServiceId, ct);
        if (hairdresserService is null)
        {
            logger.LogWarning("Hairdresser service with ID {ServiceId} not found or inactive.", request.ServiceId);
            throw new NotFoundException("Služba kadeřníka nenalezena.");
        }

        var startAt = request.StartAt;
        var endAt = startAt.AddMinutes(hairdresserService.DurationMinutes);

        if (endAt.Date != startAt.Date)
        {
            logger.LogWarning("Reservation crosses midnight. HairdresserId: {HairdresserId}, StartAt: {StartAt}, EndAt: {EndAt}", request.HairdresserId, startAt, endAt);
            throw new DomainException("Rezervace musí být v rámci jednoho dne.");
        }

        var day = startAt.DayOfWeek;
        var workingHours = await unitOfWork.HairdresserWorkingHoursRepository.GetForDayAsync(request.HairdresserId, day, ct);

        if (workingHours is null)
        {
            logger.LogWarning("Working hours not set. HairdresserId: {HairdresserId}, DayOfWeek: {DayOfWeek}, StartAt: {StartAt}", request.HairdresserId, day, startAt);
            throw new ConflictException("Kadeřník nemá pro tento den nastavenou pracovní dobu.");
        }

        var startTime = TimeOnly.FromDateTime(startAt);
        var endTime = TimeOnly.FromDateTime(endAt);

        if (startTime < workingHours.WorkFrom || endTime > workingHours.WorkTo)
        {
            logger.LogWarning(
                "Reservation is outside working hours. HairdresserId: {HairdresserId}, DayOfWeek: {DayOfWeek}, Requested: {StartTime}-{EndTime}, WorkingHours: {WorkFrom}-{WorkTo}",
                    request.HairdresserId, day, startTime, endTime, workingHours.WorkFrom, workingHours.WorkTo);

            throw new DomainException("Rezervace je mimo pracovní dobu kadeřníka.");
        }

        var hasTimeOff = await unitOfWork.HairdresserTimeOffRepository.ExistsOverlapAsync(request.HairdresserId, startAt, endAt, ct);

        if (hasTimeOff)
        {
            logger.LogWarning("Reservation overlaps hairdresser time off. HairdresserId: {HairdresserId}, StartAt: {StartAt}, EndAt: {EndAt}",
                request.HairdresserId, startAt, endAt);

            throw new ConflictException("Kadeřník má v tomto čase volno.");
        }

        var existsAnyReservation = await unitOfWork.ReservationRepository.ExistsOverlapForHairdresserAsync(request.HairdresserId, startAt, endAt, ct);
        if (existsAnyReservation)
        {
            logger.LogWarning("Reservation time slot already booked. HairdresserId: {HairdresserId}, StartAt: {StartAt}, EndAt: {EndAt}", request.HairdresserId, startAt, endAt);
            throw new ConflictException("V tomto čase již existuje rezervace.");
        }

        string? customerId;
        string? customerName;
        string? customerEmail;
        string? customerPhone;

        if (currentAppUser.User is not null)
        {
            customerId = currentAppUser.User.Id;
            customerName = currentAppUser.User.FullName;
            customerEmail = currentAppUser.User.Email ?? request.CustomerEmail;
            customerPhone = currentAppUser.User.PhoneNumber ?? request.CustomerPhone;
        }
        else
        {
            customerId = null;
            customerName = request.CustomerName;
            customerEmail = request.CustomerEmail;
            customerPhone = request.CustomerPhone;
        }

        var reservation = new BarberReservation.Domain.Entities.Reservation
        {
            HairdresserId = request.HairdresserId,
            HairdresserServiceId = hairdresserService.Id,
            StartAt = startAt,
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

        await emailService.SendReservationConfirmationEmailAsync(customerEmail,
            startAt,
            user.FullName,
            hairdresserService.Service.Name,
            hairdresserService.Price,
            hairdresserService.DurationMinutes,
            ct);

        logger.LogInformation(
            "Reservation with ID {ReservationId} created by client. HairdresserId: {HairdresserId}, CustomerName: {CustomerName}, StartAt: {StartAt}.",
            reservation.Id,
            request.HairdresserId,
            reservation.CustomerName,
            reservation.StartAt);
    }
}
