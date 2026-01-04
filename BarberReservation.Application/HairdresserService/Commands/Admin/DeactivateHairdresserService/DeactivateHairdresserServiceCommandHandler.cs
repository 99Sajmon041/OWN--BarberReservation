using BarberReservation.Application.Exceptions;
using BarberReservation.Application.HairdresserService.Commands.Admin.NewFolder;
using BarberReservation.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserService.Commands.Admin.DeactivateHairdresserService;

public sealed class DeactivateHairdresserServiceCommandHandler(
    ILogger<DeactivateHairdresserServiceCommandHandler> logger,
    IUnitOfWork unitOfWork) : IRequestHandler<DeactivateHairdresserServiceCommand>
{
    public async Task<Unit> Handle(DeactivateHairdresserServiceCommand request, CancellationToken ct)
    {
        var hairdresserService = await unitOfWork.HairdresserServiceRepository.GetByIdForAdminAsync(request.Id, ct);
        if (hairdresserService is null)
        {
            logger.LogWarning("Hairdresser service with ID: {HairdresserServiceId} was not found.", request.Id);
            throw new NotFoundException("Služba kadeřníka nebyla nalezena.");
        }

        var changed = unitOfWork.HairdresserServiceRepository.Deactivate(hairdresserService);
        if(changed)
        {
            await unitOfWork.SaveChangesAsync(ct);
            logger.LogInformation("Hairdresser service with ID: {HairdresserServiceId} was deactivated.", request.Id);
        }

        return Unit.Value;
    }
}
