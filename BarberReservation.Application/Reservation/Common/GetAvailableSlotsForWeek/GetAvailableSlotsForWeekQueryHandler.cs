using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Reservation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Reservation.Common.GetAvailableSlotsForWeek;

public sealed class GetAvailableSlotsForWeekQueryHandler(
    ILogger<GetAvailableSlotsForWeekQueryHandler> logger,
    IUnitOfWork unitOfWork)
    : IRequestHandler<GetAvailableSlotsForWeekQuery, IReadOnlyList<SlotDto>>
{
    private const int StepMinutes = 30;

    public async Task<IReadOnlyList<SlotDto>> Handle(GetAvailableSlotsForWeekQuery request, CancellationToken ct)
    {
        var existsService = await unitOfWork.HairdresserServiceRepository.ExistsActiveWithSameServiceAsync(request.HairdresserId, request.ServiceId, ct);

        if (!existsService)
        {
            logger.LogWarning("Hairdresser or service does not exist. ServiceId: {ServiceId}, HairdresserId: {HairdresserId}.", request.ServiceId, request.HairdresserId);
            throw new NotFoundException("Služba nebo kadeřník neexistuje.");
        }

        var weekStartDate = DateOnly.FromDateTime(request.WeekStartDate);

        var workHoursWeek = await unitOfWork.HairdresserWorkingHoursRepository
            .GetWeekAsync(request.HairdresserId, weekStartDate, includeHairdresser: false, tracked: false, ct);

        if (workHoursWeek.Count == 0)
        {
            logger.LogWarning("Hairdresser does not have set working hours for the week starting at {WeekStartDate}. HairdresserId: {HairdresserId}.",
                request.WeekStartDate,
                request.HairdresserId);

            throw new NotFoundException("Kadeřník nemá nastavenou pracovní dobu, nelze načíst pracovní dny.");
        }

        var serviceDuration = await unitOfWork.HairdresserServiceRepository.GetTimeDurationByHairdresserIdAndServiceIdAsync(request.ServiceId, request.HairdresserId, ct);

        if (serviceDuration <= 0)
        {
            logger.LogWarning("Service duration is invalid: {ServiceDuration}. ServiceId: {ServiceId}, HairdresserId: {HairdresserId}.",
                serviceDuration,
                request.ServiceId,
                request.HairdresserId);

            throw new ValidationException("Délka služby není platná.");
        }

        var weekStart = request.WeekStartDate.Date;
        var weekEndExclusive = weekStart.AddDays(5);

        var timeOffs = await unitOfWork.HairdresserTimeOffRepository.GetAllWeeklyAsync(request.HairdresserId, weekStart, ct);
        var reservations = await unitOfWork.ReservationRepository.GetAllWeeklyAsync(request.HairdresserId, weekStart, ct);

        var now = DateTime.Now;

        var slots = new List<SlotDto>(capacity: 256);

        for (var i = 0; i < workHoursWeek.Count; i++)
        {
            var wh = workHoursWeek[i];
            if (!wh.IsWorkingDay)
                continue;

            var day = weekStartDate.AddDays(i);
            var from = wh.WorkFrom;
            var to = wh.WorkTo;

            if (from >= to)
                continue;

            for (var start = from; start.AddMinutes(serviceDuration) <= to; start = start.AddMinutes(StepMinutes))
            {
                var startAt = new DateTime(day, start);
                var endAt = startAt.AddMinutes(serviceDuration);

                if (startAt <= now)
                    continue;

                if (startAt < weekStart || startAt >= weekEndExclusive)
                    continue;

                if (timeOffs.Count > 0 && timeOffs.Any(t => IsOverlapping(startAt, endAt, t.StartAt, t.EndAt)))
                    continue;

                if (reservations.Count > 0 && reservations.Any(r => IsOverlapping(startAt, endAt, r.StartAt, r.EndAt)))
                    continue;

                slots.Add(new SlotDto(startAt, endAt));
            }
        }

        return slots;
    }

    private static bool IsOverlapping(DateTime slotStart, DateTime slotEnd, DateTime actionStart, DateTime actionEnd)
    {
        return slotStart < actionEnd && actionStart < slotEnd;
    }
}