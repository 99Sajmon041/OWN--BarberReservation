using BarberReservation.Application.HairdresserWorkingHours.Mapping;
using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserWorkingHours.Commands.Hairdresser.UpsertSelfWorkingHours;

public sealed class UpsertSelfWorkingHoursCommandHandler(
    ILogger<UpsertSelfWorkingHoursCommandHandler> logger,
    ICurrentAppUser currentAppUser,
    IUnitOfWork unitOfWork) : IRequestHandler<UpsertSelfWorkingHoursCommand>
{
    public async Task<Unit> Handle(UpsertSelfWorkingHoursCommand request, CancellationToken ct)
    {
        var currrentDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var hairdresser = currentAppUser.User;
        var daysToMonday = ((int)DayOfWeek.Monday - (int)currrentDate.DayOfWeek + 7) % 7;
        var nextMonday = daysToMonday == 0 ? 7: daysToMonday;
        const int WeeksDelay = 3;

        var hairdresserWorkingDays = await unitOfWork.HairdresserWorkingHoursRepository.GetWeekAsync(
            hairdresser.Id,
            currrentDate,
            true,
            true,
            ct);

        if(!hairdresserWorkingDays.Any())
        {
            var days = request.DaysOfWorkingWeek.ToHairdresserWorkingHours(hairdresser.Id, currrentDate.AddDays(nextMonday));

            unitOfWork.HairdresserWorkingHoursRepository.AddDaysToWorkingWeek(days);

            logger.LogInformation("Hairdresser set up own working days. Hairdresser ID: {HairdresserId}", hairdresser.Id);
        }
        else
        {
            var upcomingChangeDate = currrentDate.AddDays(nextMonday + (7 * WeeksDelay));
            var existingUpatedWeek = await unitOfWork.HairdresserWorkingHoursRepository.GetWeekByEffectiveFromAsync(hairdresser.Id, upcomingChangeDate, ct);

            if(existingUpatedWeek.Count > 0 && existingUpatedWeek.Count != 5)
            {
                logger.LogWarning("Existing days to update count does not match to 5. Failed do update. hairdresser ID: {HairdresserId},Start date of week: {StartWeekDate}",
                    hairdresser.Id,
                    upcomingChangeDate);
                throw new DomainException("Problém při úpravě dnů, některé dny chybí. Kontaktujte administrátora webu.");
            }

            if(existingUpatedWeek.Count == 5)
            {
                var newDaysToUpdate = request.DaysOfWorkingWeek.ToDictionary(x => x.DayOfWeek);

                foreach(var day in existingUpatedWeek)
                {
                    if(newDaysToUpdate.TryGetValue(day.DayOfWeek, out var newDay))
                    {
                        day.WorkFrom = newDay.WorkFrom;
                        day.WorkTo = newDay.WorkTo;
                        day.IsWorkingDay = newDay.IsWorkingDay;
                    }
                }

                logger.LogInformation("Hairdresser updated own working days valid from: {EffectiveFrom}. Hairdresser ID: {HairdresserId}",
                    existingUpatedWeek.First().EffectiveFrom.ToString("yyyy-MM-dd"),
                    hairdresser.Id);
            }
            else
            {
                var days = request.DaysOfWorkingWeek.ToHairdresserWorkingHours(hairdresser.Id, upcomingChangeDate);

                unitOfWork.HairdresserWorkingHoursRepository.AddDaysToWorkingWeek(days);

                logger.LogInformation("Hairdresser updated own working days valid from: {EffectiveFrom}. Hairdresser ID: {HairdresserId}",
                    days.First().EffectiveFrom.ToString("yyyy-MM-dd"),
                    hairdresser.Id);
            }
        }

        await unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
