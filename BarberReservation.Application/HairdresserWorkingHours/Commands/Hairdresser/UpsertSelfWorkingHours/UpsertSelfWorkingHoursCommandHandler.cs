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
        var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var hairdresser = currentAppUser.User;
        const int WeeksDelay = 3;

        var hairdresserWorkingDays = await unitOfWork.HairdresserWorkingHoursRepository.GetWeekAsync(
            hairdresser.Id,
            currentDate,
            true,
            true,
            ct);

        if(!hairdresserWorkingDays.Any())
        {
            var daysSinceMonday = ((int)currentDate.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            var mondayOfThisWeek = currentDate.AddDays(-daysSinceMonday);

            var days = request.DaysOfWorkingWeek.ToHairdresserWorkingHours(hairdresser.Id, mondayOfThisWeek);

            unitOfWork.HairdresserWorkingHoursRepository.AddDaysToWorkingWeek(days);

            logger.LogInformation("Hairdresser set up own working days. Hairdresser ID: {HairdresserId}", hairdresser.Id);
        }
        else
        {
            var daysToMonday = ((int)DayOfWeek.Monday - (int)currentDate.DayOfWeek + 7) % 7;
            var nextMonday = daysToMonday == 0 ? 7 : daysToMonday;

            var upcomingChangeDate = currentDate.AddDays(nextMonday + (7 * WeeksDelay));
            var existingUpatedWeek = await unitOfWork.HairdresserWorkingHoursRepository.GetWeekByEffectiveFromAsync(hairdresser.Id, upcomingChangeDate, ct);

            if(existingUpatedWeek.Count > 0 && existingUpatedWeek.Count != 5)
            {
                logger.LogWarning("Existing days to update count does not match to 5. Failed do update. hairdresser ID: {HairdresserId},Start date of week: {StartWeekDate}",
                    hairdresser.Id,
                    upcomingChangeDate);
                throw new DomainException("Problém při úpravě dnů, některé dny chybí. Kontaktujte administrátora webu.");
            }

            var timeOff = await unitOfWork.HairdresserTimeOffRepository.GetTimeOffFromDateAsync(hairdresser.Id, upcomingChangeDate, ct);
            var newDaysToUpdate = request.DaysOfWorkingWeek.ToDictionary(x => x.DayOfWeek);

            foreach (var freeDay in timeOff)
            {
                if (!newDaysToUpdate.TryGetValue(freeDay.StartAt.DayOfWeek, out var existingDay))
                    continue;

                var freeStart = TimeOnly.FromDateTime(freeDay.StartAt);
                var freeEnd = TimeOnly.FromDateTime(freeDay.EndAt);

                if (!existingDay.IsWorkingDay)
                {
                    logger.LogWarning(
                        "Working hours update blocked: attempting to set non-working day while time off exists. " +
                        "HairdresserId: {HairdresserId}, Day: {DayOfWeek}, TimeOff: {StartAt} - {EndAt}, EffectiveFrom: {EffectiveFrom}",
                        hairdresser.Id,
                        freeDay.StartAt.DayOfWeek,
                        freeDay.StartAt,
                        freeDay.EndAt,
                        upcomingChangeDate);

                    throw new ConflictException(
                        $"Nelze nastavit den na nepracovní, máte naplánované volno: {freeDay.StartAt:dd.MM. HH:mm} - {freeDay.EndAt:HH:mm}. Nejdřív ho odstraňte.");
                }

                var overlaps = existingDay.WorkFrom < freeEnd && freeStart < existingDay.WorkTo;

                if (overlaps)
                {
                    logger.LogWarning(
                        "Working hours update blocked due to overlap with time off. " +
                        "HairdresserId: {HairdresserId}, Day: {DayOfWeek}, TimeOff: {StartAt} - {EndAt}, NewWork: {WorkFrom} - {WorkTo}, EffectiveFrom: {EffectiveFrom}",
                        hairdresser.Id,
                        freeDay.StartAt.DayOfWeek,
                        freeDay.StartAt,
                        freeDay.EndAt,
                        existingDay.WorkFrom,
                        existingDay.WorkTo,
                        upcomingChangeDate);

                    throw new ConflictException(
                        $"Nelze upravit pracovní dobu. {freeDay.StartAt:dd.MM. HH:mm} - {freeDay.EndAt:HH:mm} máte naplánované volno, které se s pracovní dobou překrývá.");
                }
            }

            if (existingUpatedWeek.Count == 5)
            {
                foreach (var day in existingUpatedWeek)
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
