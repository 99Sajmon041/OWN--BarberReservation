using AutoMapper;
using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Service.Commands.PartlyUpdateService;

public sealed class PartlyUpdateServiceCommandHandler(
    ILogger<PartlyUpdateServiceCommandHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<PartlyUpdateServiceCommand>
{
    public async Task Handle(PartlyUpdateServiceCommand request, CancellationToken ct)
    {
        var service = await unitOfWork.ServiceRepository.GetByIdAsync(request.Id, ct);
        if (service is null)
        {
            logger.LogWarning("Service by ID: {ServiceId} was not found.", request.Id);
            throw new NotFoundException("Služba nebyla nalezena.");
        }

        mapper.Map(request, service);

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Service with ID: {ServiceId} was updated by admin successfuly.", request.Id);
    }
}
