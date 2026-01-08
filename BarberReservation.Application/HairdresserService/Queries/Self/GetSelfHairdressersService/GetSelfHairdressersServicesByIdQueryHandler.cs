using AutoMapper;
using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.HairdresserService.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserService.Queries.Self.GetSelfHairdressersService;

public sealed class GetSelfHairdressersServicesByIdQueryHandler(
    ILogger<GetSelfHairdressersServicesByIdQueryHandler> logger,
    ICurrentAppUser currentAppUser,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetSelfHairdresserServiceByIdQuery, HairdresserServiceDto>
{
    public async Task<HairdresserServiceDto> Handle(GetSelfHairdresserServiceByIdQuery request, CancellationToken ct)
    {
        var currentUser = currentAppUser.User;

        var hairdresserService = await unitOfWork.HairdresserServiceRepository.GetByIdForCurrentUserAsync(request.Id, currentUser.Id, ct);
        if(hairdresserService is null)
        {
            logger.LogWarning("Hairdresser service with ID: {HairdresserServiceId} was not found.", request.Id);
            throw new NotFoundException("Služba nebyla nalezena.");
        }

        var hairdresserServiceDto = mapper.Map<HairdresserServiceDto>(hairdresserService);

        logger.LogInformation("Hairdresser fetched own service with ID: {HairdresserServiceId}.", request.Id);

        return hairdresserServiceDto;
    }
}
