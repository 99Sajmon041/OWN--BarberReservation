using BarberReservation.Application.HairdresserWorkingHours.Commands.Hairdresser.UpsertSelfWorkingHours;
using BarberReservation.Shared.Models.HairdresserWorkingHours;

namespace BarberReservation.Application.HairdresserWorkingHours.Mapping;

public static class WorkingHoursRequestMappingExtensions
{
    public static List<UpsertHairdresserWorkingHoursDto> ToUpsertHairdresserWorkingHoursDto(this HairdresserWorkingHoursUpsertDto request)
    {
        var result = request.DaysOfWorkingWeek.Select(x => new UpsertHairdresserWorkingHoursDto
        {
            DayOfWeek = x.DayOfWeek,
            IsWorkingDay = x.IsWorkingDay,
            WorkFrom = x.WorkFrom,
            WorkTo = x.WorkTo
        })
        .ToList();

        return result;
    }

    public static List<BarberReservation.Domain.Entities.HairdresserWorkingHours> ToHairdresserWorkingHours(
        this IEnumerable<UpsertHairdresserWorkingHoursDto> request,
        string hairdresserId,
        DateOnly effectiveFrom)
    {
        return request.Select(x => new Domain.Entities.HairdresserWorkingHours
        {
            DayOfWeek = x.DayOfWeek,
            EffectiveFrom = effectiveFrom,
            IsWorkingDay = x.IsWorkingDay,
            WorkFrom = x.WorkFrom,
            WorkTo = x.WorkTo,
            HairdresserId = hairdresserId
        })
        .ToList();
    }
}
