using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserService.Commands.Admin.DeleteHairdresserService;

public sealed class DeleteHairdresserServiceCommandHandler(
    ILogger<DeleteHairdresserServiceCommandHandler> logger,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteHairdresserServiceCommand>
{
    public async Task Handle(DeleteHairdresserServiceCommand request, CancellationToken ct)
    {
        var hairdresserService = await unitOfWork.HairdresserServiceRepository.GetByIdForAdminAsync(request.Id, ct);
        if (hairdresserService is null)
        {
            logger.LogWarning("Hairdresser service with ID: {HairdresserServiceId} was not found.", request.Id);
            throw new NotFoundException("Služba kadeřníka nebyla nalezena.");
        }

        var existsByHairdresserService = await unitOfWork.ReservationRepository.ExistsByHairDresserServiceIdAsync(request.Id, ct);
        if(existsByHairdresserService)
        {
            logger.LogWarning("Delete hairdresser service with ID: {HairdresserServiceId} is not allowed, exists reservation with same service.", request.Id);
            throw new ConflictException("Není povoleno smazat službu kadeřníka, existuje na ni vázaná rezervace.");
        }

        unitOfWork.HairdresserServiceRepository.Delete(hairdresserService);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Admin deleted hairdresser service with ID: {HairdresserServiceId}.", request.Id);
    }
}
