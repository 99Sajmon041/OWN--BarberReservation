using AutoMapper;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Service.Queries.GetAllServices;

public sealed class GetAllServicesQueryHandler(
    ILogger<GetAllServicesQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllServicesQuery, PagedResult<ServiceDto>>
{
    public async Task<PagedResult<ServiceDto>> Handle(GetAllServicesQuery request, CancellationToken ct)
    {
        var (services, totalItemsCount) = await unitOfWork.ServiceRepository.GetAllAsync(
            request.Page,
            request.PageSize,
            request.IsActive,
            request.Search,
            request.SortBy,
            request.Desc,
            ct);

        var servicesDto = mapper.Map<IReadOnlyList<ServiceDto>>(services);

        logger.LogInformation("Admin fetched filtered {ItemsCount} records of services.", totalItemsCount);

        return new PagedResult<ServiceDto>
        {
            Items = servicesDto,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItemsCount = totalItemsCount
        };
    }
}
