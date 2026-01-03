using AutoMapper;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.HairdresserService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserService.Queries.Admin.GetAllHairdresserServices;

public sealed class GetAllHairdressersServicesQueryHandler(
    ILogger<GetAllHairdressersServicesQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllHairdresserServicesQuery, PagedResult<HairdresserServiceDto>>
{
    public async Task<PagedResult<HairdresserServiceDto>> Handle(GetAllHairdresserServicesQuery request, CancellationToken ct)
    {
        var (items, total) = await unitOfWork.HairdresserServiceRepository.GetAllPagedForAdminAsync(
            request.Page,
            request.PageSize,
            request.HairdresserId,
            request.ServiceId,
            request.SortBy,
            request.Desc,
            request.IsActive,
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
