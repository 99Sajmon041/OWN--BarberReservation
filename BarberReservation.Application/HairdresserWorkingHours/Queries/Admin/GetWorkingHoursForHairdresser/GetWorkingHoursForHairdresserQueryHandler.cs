using AutoMapper;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.HairdresserWorkingHours.Admin;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Admin.GetWorkingHoursForHairdresser;

public sealed class GetWorkingHoursForHairdresserQueryHandler(
    ILogger<GetWorkingHoursForHairdresserQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetWorkingHoursForHairdresserQuery, IReadOnlyList<AdminHairdresserWorkingHoursDto>>
{
    public async Task<IReadOnlyList<AdminHairdresserWorkingHoursDto>> Handle(GetWorkingHoursForHairdresserQuery request, CancellationToken ct)
    {
        var hairdresserWorkingHours = await unitOfWork.HairdresserWorkingHoursRepository.GetAllDaysInWeekForHairdresser(
            request.HairdresserId,
            false,
            true,
            ct);

        var hairdresserWorkingHoursDto = mapper.Map<List<AdminHairdresserWorkingHoursDto>>(hairdresserWorkingHours);

        logger.LogInformation("Admin fetched hairdressers working hours per week. Hairdresser ID: {hairdresserId}", request.HairdresserId);

        return hairdresserWorkingHoursDto;
    }
}
