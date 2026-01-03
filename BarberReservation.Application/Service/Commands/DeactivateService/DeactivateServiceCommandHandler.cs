using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Service.Commands.DeactivateService;

public sealed class DeactivateServiceCommandHandler(
    ILogger<DeactivateServiceCommandHandler> logger,
    IUnitOfWork unitOfWork) : IRequestHandler<DeactivateServiceCommand>
{
    public async Task<Unit> Handle(DeactivateServiceCommand request, CancellationToken ct)
    {
        var service = await unitOfWork.ServiceRepository.GetByIdAsync(request.Id, ct);
        if (service is null)
        {
            logger.LogWarning("Service with ID: {ServiceId} was not found.", request.Id);
            throw new NotFoundException("Služba nenalezena.");
        }

        bool existsByService = await unitOfWork.HairdresserServiceRepository.ExistsByServiceIdAsync(request.Id, ct);
        if(existsByService)
        {
            logger.LogWarning("Failed to deactivate service with ID: {ServiceId}, is in use by hairdressers.", request.Id);
            throw new ConflictException("Služba nelze deaktivovat, je využívaná kadeřníky.");
        }

        await unitOfWork.ServiceRepository.DeactivateAsync(service, ct);

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Service with ID: {ServiceId} was deactivated.", request.Id);

        return Unit.Value;
    }
}
