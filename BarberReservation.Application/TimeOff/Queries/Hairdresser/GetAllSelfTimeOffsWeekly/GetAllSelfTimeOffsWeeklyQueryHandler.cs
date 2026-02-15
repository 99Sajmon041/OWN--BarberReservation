using AutoMapper;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.TimeOff;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.TimeOff.Queries.Hairdresser.GetAllSelfTimeOffsWeekly;

public sealed class GetAllSelfTimeOffsWeeklyQueryHandler(
    ILogger<GetAllSelfTimeOffsWeeklyQueryHandler> logger,
    ICurrentAppUser currentAppUser,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllSelfTimeOffsWeeklyQuery, List<HairdresserTimeOffDto>>
{
    public async Task<List<HairdresserTimeOffDto>> Handle(GetAllSelfTimeOffsWeeklyQuery request, CancellationToken ct)
    {
        var hairdresserId = currentAppUser.User.Id;

        var timeOffs = await unitOfWork.HairdresserTimeOffRepository.GetAllWeeklyAsync(hairdresserId, request.WeekStartDate, ct);
        var timeOffsDto = mapper.Map<List<HairdresserTimeOffDto>>(timeOffs);

        logger.LogInformation("Hairdresser requested weekly time off. HairdresserId: {HairdresserId}, WeekStartDate: {WeekStartDate}, Records: {Count}",
            hairdresserId,
            request.WeekStartDate,
            timeOffs.Count);

        return timeOffsDto;
    }
}
