using AutoMapper;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.HairdresserWorkingHours.Hairdresser;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Hairdresser.GetSelfWorkingHours;

public sealed class GetSelfWorkingHoursQueryHandler(
    ILogger<GetSelfWorkingHoursQueryHandler> logger,
    ICurrentAppUser currentAppUser,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetSelfWorkingHoursQuery, IReadOnlyList<HairdresserWorkingHoursDto>>
{
    public async Task<IReadOnlyList<HairdresserWorkingHoursDto>> Handle(GetSelfWorkingHoursQuery request, CancellationToken ct)
    {
        var hairdresserWorkingHours = await unitOfWork.HairdresserWorkingHoursRepository.GetAllDaysInWeekForHairdresser(
            currentAppUser.User.Id,
            false,
            true,
            ct);

        var hairdressersWorkingHoursDto = mapper.Map<List<HairdresserWorkingHoursDto>>(hairdresserWorkingHours);

        logger.LogInformation("Hairdresser fetched own working hours per week. Hairdresser ID: {hairdresserId}", currentAppUser.User.Id);

        return hairdressersWorkingHoursDto;
    }
}
