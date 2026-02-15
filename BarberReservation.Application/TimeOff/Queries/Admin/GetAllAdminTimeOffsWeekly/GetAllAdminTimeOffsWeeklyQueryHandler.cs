using AutoMapper;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.TimeOff;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.TimeOff.Queries.Admin.GetAllAdminTimeOffsWeekly;

public sealed class GetAllAdminTimeOffsWeeklyQueryHandler(
    ILogger<GetAllAdminTimeOffsWeeklyQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllAdminTimeOffsWeeklyQuery, List<HairdresserTimeOffDto>>
{
    public async Task<List<HairdresserTimeOffDto>> Handle(GetAllAdminTimeOffsWeeklyQuery request, CancellationToken ct)
    {
        var timeOffs = await unitOfWork.HairdresserTimeOffRepository.GetAllWeeklyAsync(request.HairdresserId, request.WeekStartDate, ct);

        var timeOffsDto = mapper.Map<List<HairdresserTimeOffDto>>(timeOffs);

        logger.LogInformation("Admin requested weekly time off. HairdresserId: {HairdresserId}, WeekStartDate: {WeekStartDate}, Records: {Count}",
            request.HairdresserId,
            request.WeekStartDate,
            timeOffs.Count);

        return timeOffsDto;
    }
}
