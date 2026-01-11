using AutoMapper;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.LookUpModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Service.Queries.GetLookUpServices;

public sealed class GetLookUpServicesQueryHandler(
    ILogger<GetLookUpServicesQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetLookUpServicesQuery, IReadOnlyList<ServiceLookUpDto>>
{
    public async Task<IReadOnlyList<ServiceLookUpDto>> Handle(GetLookUpServicesQuery request, CancellationToken ct)
    {
        var services = await unitOfWork.ServiceRepository.GetAllLookUpAsync(ct);

        var servicesDto = mapper.Map<IReadOnlyList<ServiceLookUpDto>>(services);

        logger.LogInformation("Retrieved {Count} services for lookup.", servicesDto.Count);

        return servicesDto;
    }
}
