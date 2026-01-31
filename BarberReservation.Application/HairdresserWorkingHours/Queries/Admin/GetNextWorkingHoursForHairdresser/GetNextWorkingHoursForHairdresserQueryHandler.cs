using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.HairdresserWorkingHours;
using BarberReservation.Shared.Models.HairdresserWorkingHours.Hairdresser;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Admin.GetNextWorkingHoursForHairdresser;

public sealed class GetNextWorkingHoursForHairdresserQueryHandler(
    ILogger<GetNextWorkingHoursForHairdresserQueryHandler> logger,
    IUnitOfWork unitOfWork,
    UserManager<ApplicationUser> userManager) : IRequestHandler<GetNextWorkingHoursForHairdresserQuery, HairdresserWorkingHoursDto>
{
    public async Task<HairdresserWorkingHoursDto> Handle(GetNextWorkingHoursForHairdresserQuery request, CancellationToken ct)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

        var response = await unitOfWork.HairdresserWorkingHoursRepository.GetNextWeekAsync(
            request.HairdresserId,
            currentDate,
            true,
            false,
            ct);

        if (response.Count == 0)
        {
            var hairdresser = await userManager.FindByIdAsync(request.HairdresserId);

            return new HairdresserWorkingHoursDto
            {
                HairdresserName = hairdresser?.FullName ?? string.Empty,
                EffectiveFrom = default,
                WorkingHours = []
            };
        }

        var first = response[0];

        var dto = new HairdresserWorkingHoursDto
        {
            EffectiveFrom = first.EffectiveFrom,
            HairdresserName = $"{first.Hairdresser.FirstName} {first.Hairdresser.LastName}",
            WorkingHours = response.Select(x => new WorkingHoursDto
            {
                DayOfWeek = x.DayOfWeek,
                IsWorkingDay = x.IsWorkingDay,
                WorkFrom = x.WorkFrom,
                WorkTo = x.WorkTo
            }).ToList()
        };

        logger.LogInformation("Admin fetched hairdresser up-coming working hours per week. Hairdresser ID: {hairdresserId}", request.HairdresserId);

        return dto;
    }
}
