using AutoMapper;
using BarberReservation.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Service.Commands.CreateService;

public sealed class CreateServiceCommandHandler(
    ILogger<CreateServiceCommandHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreateServiceCommand, int>
{
    public async Task<int> Handle(CreateServiceCommand request, CancellationToken ct)
    {
        var service = mapper.Map<BarberReservation.Domain.Entities.Service>(request);
        service.IsActive = true;

        await unitOfWork.ServiceRepository.CreateAsync(service, ct);

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Admin created new Service with name: {ServiceName}.", request.Name);

        return service.Id;
    }
}
