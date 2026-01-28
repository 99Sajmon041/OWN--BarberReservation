using AutoMapper;
using BarberReservation.Application.HairdresserService.Mapping;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.HairdresserService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserService.Queries.Admin.GetAllAdminHairdressersServices;

public sealed class GetAllSelfHairdressersServicesQueryHandler(
    ILogger<GetAllSelfHairdressersServicesQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllAdminHairdressersServicesQuery, PagedResult<HairdresserServiceDto>>
{
    public async Task<PagedResult<HairdresserServiceDto>> Handle(GetAllAdminHairdressersServicesQuery request, CancellationToken ct)
    {
        var hairdresserAdminServicePagedRequest = request.ToHairdresserAdminServicePagedRequest();

        var (items, total) = await unitOfWork.HairdresserServiceRepository.GetAllPagedForAdminAsync(hairdresserAdminServicePagedRequest, ct);

        var hairdresserServicesDtos = mapper.Map<IReadOnlyList<HairdresserServiceDto>>(items);

        logger.LogInformation("Admin fetched filtered {ItemsCount} records of hairdressers services.", items.Count);

        return new PagedResult<HairdresserServiceDto>
        {
            Items = hairdresserServicesDtos,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItemsCount = total
        };
    }
}
