using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserService.Commands.Self.ActivateSelfHairdresserService;

public sealed class ActivateSelfHairdresserServiceCommandHandler(
    ILogger<ActivateSelfHairdresserServiceCommandHandler> logger,
    ICurrentAppUser currentAppUser,
    IUnitOfWork unitOfWork) : IRequestHandler<ActivateSelfHairdresserServiceCommand>
{
    public async Task Handle(ActivateSelfHairdresserServiceCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var hairdresser = currentAppUser.User;

        var hairdresserService = await unitOfWork.HairdresserServiceRepository.GetByIdForCurrentUserAsync(request.Id, hairdresser.Id, ct);
        if (hairdresserService is null)
        {
            logger.LogWarning("Hairdresser service with ID {HairdresserServiceId} not found for hairdresser ID {HairdresserId}.", request.Id, hairdresser.Id);
            throw new NotFoundException("Hairdresser service not found.");
        }

        if (hairdresserService.IsActive)
            return;

        hairdresserService.IsActive = true;

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Hairdresser service with ID {HairdresserServiceId} was reactivated by hairdresser ID {HairdresserId}.", request.Id, hairdresser.Id);
    }
}
