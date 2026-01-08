using AutoMapper;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.HairdresserService.Admin;
using BarberReservation.Shared.Models.HairdresserService.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserService.Queries.Admin.GetAllHairdresserServices;

public sealed class GetAllHairdressersServicesQueryHandler(
    ILogger<GetAllHairdressersServicesQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllHairdressersServicesQuery, PagedResult<HairdresserServiceDto>>
{
    public async Task<PagedResult<HairdresserServiceDto>> Handle(GetAllHairdressersServicesQuery request, CancellationToken ct)
    {
        var (items, total) = await unitOfWork.HairdresserServiceRepository.GetAllPagedForAdminAsync(new HairdresserAdminServicePagedRequest
        {
            Page = request.Page,
            PageSize = request.PageSize,
            Search = request.Search,
            HairdresserId = request.HairdresserId,
            ServiceId = request.ServiceId,
            IsActive = request.IsActive,
            SortBy = request.SortBy,
            Desc = request.Desc
        },
        ct);

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
