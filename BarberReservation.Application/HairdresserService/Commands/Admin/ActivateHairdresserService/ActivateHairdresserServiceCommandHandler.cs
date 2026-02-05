using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserService.Commands.Admin.ActivateHairdresserService;

public sealed class ActivateHairdresserServiceCommandHandler(
    ILogger<ActivateHairdresserServiceCommandHandler> logger,
    IUnitOfWork unitOfWork) : IRequestHandler<ActivateHairdresserServiceCommand>
{
    public async Task Handle(ActivateHairdresserServiceCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var hairdresserService = await unitOfWork.HairdresserServiceRepository.GetByIdForAdminAsync(request.Id, ct);
        if (hairdresserService is null)
        {
            logger.LogWarning("Hairdresser service with ID: {HairdresserServiceId} was not found.", request.Id);
            throw new NotFoundException("Služba kadeřníka nebyla nalezena.");
        }

        if (hairdresserService.IsActive)
            return;

        hairdresserService.IsActive = true;

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Hairdresser service with ID: {HairdresserServiceId} was reactivated.", request.Id);
    }
}
