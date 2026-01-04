using AutoMapper;
using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserService.Commands.Self.CreateSelfHairdresserService;

public sealed class CreateSelfHairdresserServiceCommandHandler(
    ILogger<CreateSelfHairdresserServiceCommandHandler> logger,
    ICurrentAppUser currentAppUser,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreateSelfHairdresserServiceCommand, int>
{
    public async Task<int> Handle(CreateSelfHairdresserServiceCommand request, CancellationToken ct)
    {
        var existsService = await unitOfWork.ServiceRepository.ExistsAsync(request.ServiceId, ct);
        if(!existsService)
        {
            logger.LogWarning("Service with ID: {ServiceId} does not exist.", request.ServiceId);
            throw new NotFoundException("Zvolená služba neexistuje.");
        }

        var currentUser = currentAppUser.User;

        var existSameService = await unitOfWork.HairdresserServiceRepository.ExistsActiveWithSameServiceAsync(currentUser.Id, request.ServiceId, ct);
        if(existSameService)
        {
            logger.LogWarning("Service with ID: {ServiceId} by hairdresser with ID: {HairdresserId} is in use.", request.ServiceId, currentUser.Id);
            throw new ConflictException("Tuto službu už máte aktivní. Nejdřív ji deaktivujte.");
        }

        var hairdresserService = mapper.Map<BarberReservation.Domain.Entities.HairdresserService>(request);
        hairdresserService.HairdresserId = currentUser.Id;

        await unitOfWork.HairdresserServiceRepository.CreateAsync(hairdresserService, ct);

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Hairdresser added own new service with ID: {HairdresserServiceId}", hairdresserService.Id);

        return hairdresserService.Id;
    }
}
