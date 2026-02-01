using AutoMapper;
using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.HairdresserWorkingHours;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Hairdresser.GetSelfDailyWorkingHours;

public sealed class GetSelfDailyWorkingHoursQueryHandler(
    ILogger<GetSelfDailyWorkingHoursQueryHandler> logger,
    ICurrentAppUser currentAppUser,
    IUnitOfWork unitOfWork) : IRequestHandler<GetSelfDailyWorkingHoursQuery, WorkingHoursDto>
{
    public async Task<WorkingHoursDto> Handle(GetSelfDailyWorkingHoursQuery request, CancellationToken ct)
    {
        var hairdresserId = currentAppUser.User.Id;
        var dayOfWeek = request.Day.ToDateTime(TimeOnly.MinValue).DayOfWeek;

        var workingDay = await unitOfWork.HairdresserWorkingHoursRepository.GetEffectiveForDayAsync(hairdresserId, dayOfWeek, request.Day, ct);
        if(workingDay is null)
        {
            logger.LogWarning("Hairdresser does not have set working hours. Firstly he need to set up. Hairdreser ID: {HairdresserId}", hairdresserId);
            throw new NotFoundException("Nelze vytvořit volno, nemáte nastavenou poracovní dobu. Prvně si vytvořte.");
        }

        var workDay = new WorkingHoursDto
        {
            DayOfWeek = workingDay.DayOfWeek,
            IsWorkingDay = workingDay.IsWorkingDay,
            WorkFrom = workingDay.WorkFrom,
            WorkTo = workingDay.WorkTo
        };

        return workDay;
    }
}
