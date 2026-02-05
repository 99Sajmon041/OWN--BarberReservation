using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Service.Commands.ActivateService;

public sealed class ActivateServiceCommandHandler(
    ILogger<ActivateServiceCommandHandler> logger,
    IUnitOfWork unitOfWork) : IRequestHandler<ActivateServiceCommand>
{
    public async Task Handle(ActivateServiceCommand request, CancellationToken ct)
    {
        var service = await unitOfWork.ServiceRepository.GetByIdAsync(request.Id, ct);
        if (service is null)
        {
            logger.LogWarning("Service with ID: {ServiceId} was not found.", request.Id);
            throw new NotFoundException("Služba nenalezena.");
        }

        if (service.IsActive)
            return;

        service.IsActive = true;

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Service with ID: {ServiceId} was activated.", request.Id);
    }
}
