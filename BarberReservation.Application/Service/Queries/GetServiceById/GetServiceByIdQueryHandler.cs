using AutoMapper;
using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Service.Queries.GetServiceById;

public sealed class GetServiceByIdQueryHandler(
    ILogger<GetServiceByIdQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetServiceByIdQuery, ServiceDto>
{
    public async Task<ServiceDto> Handle(GetServiceByIdQuery request, CancellationToken ct)
    {
        var service = await unitOfWork.ServiceRepository.GetByIdAsync(request.Id, ct);
        if(service is null)
        {
            logger.LogWarning("Service by ID: {ServiceId} was not found.", request.Id);
            throw new NotFoundException("Služba nebyla nalezena.");
        }

        var serviceDto = mapper.Map<ServiceDto>(service);

        logger.LogInformation("Admin fetched service with ID: {ServiceId}", request.Id);

        return serviceDto; 
    }
}
