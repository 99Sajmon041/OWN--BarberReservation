using AutoMapper;
using BarberReservation.Application.HairdresserService.Mapping;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.HairdresserService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserService.Queries.Self.GetAllSelfHairdressersServices;

public sealed class GetAllSelfHairdressersServicesQueryHandler(
    ILogger<GetAllSelfHairdressersServicesQueryHandler> logger,
    ICurrentAppUser currentAppUser,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllSelfHairdressersServicesQuery, PagedResult<HairdresserServiceDto>>
{
    public async Task<PagedResult<HairdresserServiceDto>> Handle(GetAllSelfHairdressersServicesQuery request, CancellationToken ct)
    {
        var currentUser = currentAppUser.User;

        var hairdresserSelfServicePagedRequest = request.ToHairdresserSelfServicePagedRequest();

        var (hairdresserServices, total) = await unitOfWork.HairdresserServiceRepository.GetAllPagedForCurrentUserAsync(hairdresserSelfServicePagedRequest, currentUser.Id, ct);

        var items = mapper.Map<IReadOnlyList<HairdresserServiceDto>>(hairdresserServices);

        logger.LogInformation("Hairdresser fetched filtered {ItemsCount} records of own customized services.", items.Count);

        return new PagedResult<HairdresserServiceDto>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItemsCount = total
        };
    }
}
