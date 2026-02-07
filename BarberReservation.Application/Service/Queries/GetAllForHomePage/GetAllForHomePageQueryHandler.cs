using AutoMapper;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Service.Queries.GetAllForHomePage;

public sealed class GetAllForHomePageQueryHandler(
    ILogger<GetAllForHomePageQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllForHomePageQuery, List<ServiceDto>>
{
    public async Task<List<ServiceDto>> Handle(GetAllForHomePageQuery request, CancellationToken ct)
    {
        var services = await unitOfWork.ServiceRepository.GetAllForHomePageAsync(ct);

        var serviceDtos = mapper.Map<List<ServiceDto>>(services);

        logger.LogInformation("Retrieved {Count} services for home page", serviceDtos.Count);

        return serviceDtos;
    }
}
