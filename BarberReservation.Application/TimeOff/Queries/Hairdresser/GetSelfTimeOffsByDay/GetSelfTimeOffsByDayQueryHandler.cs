using AutoMapper;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.TimeOff.Hairdresser;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.TimeOff.Queries.Hairdresser.GetSelfTimeOffsByDay;

public sealed class GetSelfTimeOffsByDayQueryHandler(
    ILogger<GetSelfTimeOffsByDayQueryHandler> logger,
    ICurrentAppUser currentAppUser,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetSelfTimeOffsByDayQuery, List<HairdresserTimeOffDto>>
{
    public async Task<List<HairdresserTimeOffDto>> Handle(GetSelfTimeOffsByDayQuery request, CancellationToken ct)
    {
        var hairdresser = currentAppUser.User;
        var day = request.Day;

        var timeOffs = await unitOfWork.HairdresserTimeOffRepository.GetAllByDayForHairdresserAsync(hairdresser.Id, day, ct);

        var timeOffDtos = mapper.Map<List<HairdresserTimeOffDto>>(timeOffs);

        logger.LogInformation("Hairdresser fetched own time-off in current day. Hairdresser ID: {HairdresserId}, Day: {Today}", hairdresser.Id, day);

        return timeOffDtos;
    }
}
