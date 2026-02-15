using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.HairdresserWorkingHours;
using BarberReservation.Application.HairdresserWorkingHours.Mapping;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Hairdresser.GetSelfWorkingHoursByWeek;

public sealed class GetSelfWorkingHoursByWeekQueryHandler(
    ILogger<GetSelfWorkingHoursByWeekQueryHandler> logger,
    ICurrentAppUser currentAppUser,
    IUnitOfWork unitOfWork) : IRequestHandler<GetSelfWorkingHoursByWeekQuery, List<WorkingHoursDto>>
{
    public async Task<List<WorkingHoursDto>> Handle(GetSelfWorkingHoursByWeekQuery request, CancellationToken ct)
    {
        var hairdresserId = currentAppUser.User.Id;

        var workingHours = await unitOfWork.HairdresserWorkingHoursRepository.GetWeekAsync(hairdresserId, request.Monday, ct);

        var workingHoursDto = workingHours.ToWorkingHoursDto();

        logger.LogInformation(
            "Admin requested weekly working hours. HairdresserId: {HairdresserId}, Monday: {Monday}, Records: {Count}",
            hairdresserId,
            request.Monday,
            workingHours.Count);

        return workingHoursDto;

    }
}
