using AutoMapper;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserWorkingHours.Commands.Hairdresser.UpsertSelfWorkingHours;

public sealed class UpsertSelfWorkingHoursCommandHandler(
    ILogger<UpsertSelfWorkingHoursCommandHandler> logger,
    ICurrentAppUser currentAppUser,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<UpsertSelfWorkingHoursCommand>
{
    public async Task<Unit> Handle(UpsertSelfWorkingHoursCommand request, CancellationToken ct)
    {
        var hairdresser = currentAppUser.User;
        int modifiedDays = 0;

        var hairdresserWorkingDays = await unitOfWork.HairdresserWorkingHoursRepository.GetAllDaysInWeekForHairdresser(hairdresser.Id, true, false, ct);

        var existingByDay = hairdresserWorkingDays.ToDictionary(x => x.DayOfWeek);

        foreach (var day in request.DaysOfWorkingWeek)
        {
            if (!existingByDay.TryGetValue(day.DayOfWeek, out var existsDay))
            {
                var newDay = mapper.Map<BarberReservation.Domain.Entities.HairdresserWorkingHours>(day);
                newDay.HairdresserId = hairdresser.Id;
                unitOfWork.HairdresserWorkingHoursRepository.AddDayToWorkingWeek(newDay);

                modifiedDays++;
            }
            else if(day.WorkFrom == existsDay.WorkFrom && day.WorkTo == existsDay.WorkTo && day.IsWorkingDay == existsDay.IsWorkingDay)
            {
                continue;
            }
            else
            {
                mapper.Map(day, existsDay);

                modifiedDays++;
            }
        }

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Hairdresser updated: {ModifiedDays} days in the week. Hairdresser ID: {HairdresserId}", modifiedDays, hairdresser.Id);

        return Unit.Value;
    }
}
