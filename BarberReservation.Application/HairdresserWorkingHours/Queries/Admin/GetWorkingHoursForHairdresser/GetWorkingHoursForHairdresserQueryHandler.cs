using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.HairdresserWorkingHours.Admin;
using BarberReservation.Shared.Models.HairdresserWorkingHours.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Admin.GetWorkingHoursForHairdresser;

public sealed class GetWorkingHoursForHairdresserQueryHandler(
    ILogger<GetWorkingHoursForHairdresserQueryHandler> logger,
    IUnitOfWork unitOfWork,
    UserManager<ApplicationUser> userManager) : IRequestHandler<GetWorkingHoursForHairdresserQuery, AdminHairdresserWorkingHoursDto>
{
    public async Task<AdminHairdresserWorkingHoursDto> Handle(GetWorkingHoursForHairdresserQuery request, CancellationToken ct)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

        var response = await unitOfWork.HairdresserWorkingHoursRepository.GetWeekAsync(
            request.HairdresserId,
            currentDate,
            true,
            false,
            ct);

        if (response.Count == 0)
        {
            var hairdresser = await userManager.FindByIdAsync(request.HairdresserId);

            return new AdminHairdresserWorkingHoursDto
            {
                HairdresserName = hairdresser?.FullName ?? string.Empty,
                EffectiveFrom = default,
                WorkingHours = []
            };
        }

        var first = response[0];

        var dto = new AdminHairdresserWorkingHoursDto
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

        logger.LogInformation("Admin fetched hairdresser working hours per week. Hairdresser ID: {hairdresserId}", request.HairdresserId);

        return dto;
    }
}
