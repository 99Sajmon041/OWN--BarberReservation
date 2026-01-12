using AutoMapper;
using BarberReservation.Domain.Entities;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.LookUpModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.User.Queries.Self.GetLookUpHairdressers;

public sealed class GetLookUpHairdressersQueryHandler(
    ILogger<GetLookUpHairdressersQueryHandler> logger,
    UserManager<ApplicationUser> userManager,
    IMapper mapper) : IRequestHandler<GetLookUpHairdressersQuery, IEnumerable<Shared.Models.LookUpModels.GetLookUpHairdressersByService>>
{
    public async Task<IEnumerable<Shared.Models.LookUpModels.GetLookUpHairdressersByService>> Handle(GetLookUpHairdressersQuery request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var hairdressers = await userManager.GetUsersInRoleAsync(nameof(UserRoles.Hairdresser));

        var filteredHairdressers = hairdressers
            .Where(x => x.IsActive)
            .OrderBy(x => x.LastName)
            .ThenBy(x => x.FirstName);

        var hairdresserLookUpDtos = mapper.Map<IEnumerable<Shared.Models.LookUpModels.GetLookUpHairdressersByService>>(filteredHairdressers);

        logger.LogInformation("Retrieved {Count} active hairdressers for lookup.", hairdresserLookUpDtos.Count());

        return hairdresserLookUpDtos;
    }
}
