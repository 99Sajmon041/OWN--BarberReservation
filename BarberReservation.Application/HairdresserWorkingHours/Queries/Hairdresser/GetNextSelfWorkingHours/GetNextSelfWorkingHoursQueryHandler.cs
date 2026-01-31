using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.HairdresserWorkingHours;
using BarberReservation.Shared.Models.HairdresserWorkingHours.Hairdresser;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Hairdresser.GetNextSelfWorkingHours;

public sealed class GetNextSelfWorkingHoursQueryHandler(
    ILogger<GetNextSelfWorkingHoursQueryHandler> logger,
    ICurrentAppUser currentAppUser,
    IUnitOfWork unitOfWork) : IRequestHandler<GetNextSelfWorkingHoursQuery, HairdresserWorkingHoursDto>
{
    public async Task<HairdresserWorkingHoursDto> Handle(GetNextSelfWorkingHoursQuery request, CancellationToken ct)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

        var response = await unitOfWork.HairdresserWorkingHoursRepository.GetNextWeekAsync(
            currentAppUser.User.Id,
            currentDate,
            false,
            false,
            ct);

        if (response.Count == 0)
        {
            return new HairdresserWorkingHoursDto
            {
                EffectiveFrom = default,
                WorkingHours = []
            };
        }

        var dto = new HairdresserWorkingHoursDto
        {
            EffectiveFrom = response[0].EffectiveFrom,
            WorkingHours = response.Select(x => new WorkingHoursDto
            {
                DayOfWeek = x.DayOfWeek,
                IsWorkingDay = x.IsWorkingDay,
                WorkFrom = x.WorkFrom,
                WorkTo = x.WorkTo
            }).ToList()
        };

        logger.LogInformation("Hairdresser fetched own up-coming working hours per week. Hairdresser ID: {HairdresserId}", currentAppUser.User.Id);

        return dto;
    }
}
