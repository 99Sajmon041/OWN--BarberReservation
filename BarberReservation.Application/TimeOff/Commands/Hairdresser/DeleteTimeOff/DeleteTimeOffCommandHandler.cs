using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.TimeOff.Commands.Hairdresser.DeleteTimeOff;

public sealed class DeleteTimeOffCommandHandler(
    ILogger<DeleteTimeOffCommandHandler> logger,
    ICurrentAppUser currentAppUser,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteTimeOffCommand>
{
    public async Task Handle(DeleteTimeOffCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var hairdresser = currentAppUser.User;

        var timeOff = await unitOfWork.HairdresserTimeOffRepository.GetByIdAsync(request.Id, hairdresser.Id, ct);
        if (timeOff == null)
        {
            logger.LogWarning(
                "Hairdresser tries to delete time off, but it was not found (or does not belong to the hairdresser). Hairdresser ID: {HairdresserId}, TimeOff ID: {TimeOffId}",
                hairdresser.Id,
                request.Id);

            throw new NotFoundException("Volno nelze smazat - nebylo nalezeno.");
        }

        var allowedTimeToModify = DateTime.UtcNow.AddHours(1);

        if (timeOff.StartAt <= allowedTimeToModify)
        {
            logger.LogWarning(
                    "Hairdresser tries to delete time off less than 1 hour before start (or already started). Hairdresser ID: {HairdresserId}," +
                    " TimeOff ID: {TimeOffId}, StartAt: {StartAt:O}",
                        hairdresser.Id,
                        request.Id,
                        timeOff.StartAt);

            throw new ConflictException("Volno nelze mazat hodinu předem nebo volno, které už proběhlo. ");
        }

        unitOfWork.HairdresserTimeOffRepository.Delete(timeOff);

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Hairdresser delete own Free with ID: {FreeId}. Hairdresser ID: {HairdresserId}", request.Id, hairdresser.Id);
    }
}
