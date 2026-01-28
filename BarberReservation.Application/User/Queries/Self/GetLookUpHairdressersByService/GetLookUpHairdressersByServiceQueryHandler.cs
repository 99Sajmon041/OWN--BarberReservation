using AutoMapper;
using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Queries.Self.GetLookUpHairdressersByService;

public sealed class GetLookUpHairdressersByServiceQueryHandler(
    ILogger<GetLookUpHairdressersByServiceQueryHandler> logger,
    UserManager<ApplicationUser> userManager,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IRequestHandler<GetLookUpHairdressersByServiceQuery, IReadOnlyList<Shared.Models.LookUpModels.GetLookUpHairdressers>>
{
    public async Task<IReadOnlyList<Shared.Models.LookUpModels.GetLookUpHairdressers>> Handle(GetLookUpHairdressersByServiceQuery request, CancellationToken ct)
    {
        var ids = await unitOfWork.HairdresserServiceRepository.GetActiveHairdresserIdsByServiceIdAsync(request.ServiceId, ct);

        var hairdressers = await userManager.Users
            .Where(u => ids.Contains(u.Id))
            .ToListAsync(ct);

        var hairdressersDto = mapper.Map<List<Shared.Models.LookUpModels.GetLookUpHairdressers>>(hairdressers);

        logger.LogInformation("Retrieved hairdressers by their service. Service ID {ServiceId}.", request.ServiceId);

        return hairdressersDto;
    }
}
