using AutoMapper;
using BarberReservation.Application.Service.Mapping;
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
        var servicePageRequest = request.ToServicePageRequest();

        var (items, total) = await unitOfWork.ServiceRepository.GetAllAsync(servicePageRequest, ct);

        var servicesDto = mapper.Map<IReadOnlyList<ServiceDto>>(items);

        logger.LogInformation("Admin fetched filtered {ItemsCount} records of services.", items.Count);

        return new PagedResult<ServiceDto>
        {
            Items = servicesDto,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItemsCount = total
        };
    }
}
