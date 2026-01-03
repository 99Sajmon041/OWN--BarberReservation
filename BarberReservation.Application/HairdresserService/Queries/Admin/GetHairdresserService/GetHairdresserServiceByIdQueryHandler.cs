using AutoMapper;
using BarberReservation.Application.Exceptions;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.HairdresserService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.HairdresserService.Queries.Admin.GetHairdresserService;

public sealed class GetHairdresserServiceByIdQueryHandler(
    ILogger<GetHairdresserServiceByIdQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetHairdresserServiceByIdQueryQuery, HairdresserServiceDto>
{
    public async Task<HairdresserServiceDto> Handle(GetHairdresserServiceByIdQueryQuery request, CancellationToken ct)
    {
        var hairdresserService = await unitOfWork.HairdresserServiceRepository.GetByIdForAdminAsync(request.Id, ct);
        if(hairdresserService is null)
        {
            logger.LogWarning("Hairdresser service with ID: {HairdresserServiceId} was not found.", request.Id);
            throw new NotFoundException("Služba kadeřníka nebyla nalezena.");
        }

        var hairdresserServiceDto = mapper.Map<HairdresserServiceDto>(hairdresserService);

        logger.LogInformation("Admin fetched Hairdresser service with ID: {HairdresserServiceId}.", request.Id);

        return hairdresserServiceDto;
    }
}
