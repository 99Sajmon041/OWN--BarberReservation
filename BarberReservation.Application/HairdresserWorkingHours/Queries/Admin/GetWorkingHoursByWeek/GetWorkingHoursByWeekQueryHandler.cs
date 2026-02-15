using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.HairdresserWorkingHours;
using MediatR;
using Microsoft.Extensions.Logging;
using BarberReservation.Application.HairdresserWorkingHours.Mapping;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Admin.GetWorkingHoursByWeek;

public sealed class GetWorkingHoursByWeekQueryHandler(
    ILogger<GetWorkingHoursByWeekQueryHandler> logger,
    IUnitOfWork unitOfWork) : IRequestHandler<GetWorkingHoursByWeekQuery, List<WorkingHoursDto>>
{
    public async Task<List<WorkingHoursDto>> Handle(GetWorkingHoursByWeekQuery request, CancellationToken ct)
    {
        var workingHours = await unitOfWork.HairdresserWorkingHoursRepository.GetWeekAsync(request.HairdresserId, request.Monday, ct);

        var workingHoursDto = workingHours.ToWorkingHoursDto();

        logger.LogInformation(
            "Admin requested weekly working hours. HairdresserId: {HairdresserId}, Monday: {Monday}, Records: {Count}",
            request.HairdresserId,
            request.Monday,
            workingHours.Count);

        return workingHoursDto;
    }
}
