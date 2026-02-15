using AutoMapper;
using BarberReservation.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Queries.Self.GetLookUpActiveHairdressers;

public sealed class GetLookUpActiveHairdressersQueryHandler(
    ILogger<GetLookUpActiveHairdressersQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetLookUpActiveHairdressersQuery, List<Shared.Models.LookUpModels.LookUpHairdressersDto>>
{
    public async Task<List<Shared.Models.LookUpModels.LookUpHairdressersDto>> Handle(GetLookUpActiveHairdressersQuery request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var hairdressers = await unitOfWork.HairdresserWorkingHoursRepository.GetActiveHairdressersWithAnyFullWeekAsync(ct);

        var hairdresserLookUpDtos = mapper.Map<List<Shared.Models.LookUpModels.LookUpHairdressersDto>>(hairdressers);

        var list = hairdresserLookUpDtos.ToList();

        logger.LogInformation("Retrieved {Count} active hairdressers for lookup with working hours.", list.Count);

        return list;
    }
}
