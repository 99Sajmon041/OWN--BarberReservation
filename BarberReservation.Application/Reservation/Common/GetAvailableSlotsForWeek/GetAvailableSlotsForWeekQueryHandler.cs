using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Reservation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Reservation.Common.GetAvailableSlotsForWeek;

public sealed class GetAvailableSlotsForWeekQueryHandler(
    ILogger<GetAvailableSlotsForWeekQueryHandler> logger,
    IUnitOfWork unitOfWork) : IRequestHandler<GetAvailableSlotsForWeekQuery, IReadOnlyList<SlotDto>>
{
    public async Task<IReadOnlyList<SlotDto>> Handle(GetAvailableSlotsForWeekQuery request, CancellationToken ct)
    {
        var existsService = await unitOfWork.HairdresserServiceRepository.ExistsActiveWithSameServiceAsync(request.HairdresserId, request.ServiceId, ct);
        if (!existsService)
        {   
            logger.LogWarning("Hairdresser or service does not exitst. Service ID: {ServiceId}, hairdresser ID: {HairdresserId}.", request.ServiceId, request.HairdresserId);
            throw new NotFoundException("Služba nebo kadeřník neexistuje.");
        }

        var weekStartDate = DateOnly.FromDateTime(request.WeekStartDate);

        var workHoursWeek = await unitOfWork.HairdresserWorkingHoursRepository.GetWeekAsync(
            request.HairdresserId,
            weekStartDate,
            includeHairdresser: false, tracked: false, ct);

        if (workHoursWeek.Count == 0)
        {
           logger.LogWarning("Hairdresser does not have set working hours for the week starting at {WeekStartDate}. Hairdresser ID: {HairdresserId}.",
               request.WeekStartDate,
               request.HairdresserId);

            throw new NotFoundException("Kadeřník nemá nastavenou pracovní dobu pro tento týden.");
        }

        var serviceDuration = await unitOfWork.HairdresserServiceRepository.GetTimeDurationByHairdresserIdAndServiceIdAsync(request.ServiceId, request.HairdresserId, ct);

        if (serviceDuration <= 0)
        {
            logger.LogWarning("Service duration is invalid: {ServiceDuration}. ServiceId: {ServiceId}, HairdresserId: {HairdresserId}",
                serviceDuration, 
                request.ServiceId,
                request.HairdresserId);

            throw new ValidationException("Délka služby není platná.");
        }

        var slots = new List<Slot>();

        for (int i = 0; i < workHoursWeek.Count; i++)
        {
            var actualDay = weekStartDate.AddDays(i);

            var counter = workHoursWeek[i].WorkFrom;
            var endAt = workHoursWeek[i].WorkTo;

            if (counter >= endAt)
                continue;

            while (counter.AddMinutes(serviceDuration) <= endAt)
            {
                var slotEnd = counter.AddMinutes(serviceDuration);

                slots.Add(new Slot
                {
                    Day = actualDay,
                    WorkFrom = counter,
                    WorkTo = slotEnd
                });

                counter = slotEnd;
            }
        }




        return new List<SlotDto>();
    }
}

public class Slot
{
    public DateOnly Day {  get; set; }
    public TimeOnly WorkFrom { get; set; }
    public TimeOnly WorkTo { get; set; }
}