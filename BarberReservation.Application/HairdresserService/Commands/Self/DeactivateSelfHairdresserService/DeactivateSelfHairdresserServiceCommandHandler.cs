using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserService.Commands.Self.DeactivateSelfHairdresserService;

public sealed class DeactivateSelfHairdresserServiceCommandHandler(
    ILogger<DeactivateSelfHairdresserServiceCommandHandler> logger,
    IUnitOfWork unitOfWork,
    ICurrentAppUser currentAppUser) : IRequestHandler<DeactivateSelfHairdresserServiceCommand>
{
    public async Task<Unit> Handle(DeactivateSelfHairdresserServiceCommand request, CancellationToken ct)
    {
        var hairdresserId = currentAppUser.User.Id;

        var hairdresserService = await unitOfWork.HairdresserServiceRepository.GetByIdForCurrentUserAsync(request.Id, hairdresserId, ct);
        if(hairdresserService is null)
        {
            logger.LogWarning("Hairdresser service with ID {HairdresserServiceId} not found for hairdresser ID {HairdresserId}.", request.Id, hairdresserId);
            throw new NotFoundException("Hairdresser service not found.");
        }

        if (!unitOfWork.HairdresserServiceRepository.Deactivate(hairdresserService))
            return Unit.Value;

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Hairdresser service with ID {HairdresserServiceId} deactivated by hairdresser ID {HairdresserId}.", request.Id, hairdresserId);
        
        return Unit.Value;
    }
}
