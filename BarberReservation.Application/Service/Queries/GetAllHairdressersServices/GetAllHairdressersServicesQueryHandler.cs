using AutoMapper;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.Service.Queries.GetAllHairdressersServices;

public sealed class GetAllHairdressersServicesQueryHandler(
    ILogger<GetAllHairdressersServicesQueryHandler> logger,
    ICurrentAppUser currentAppUser,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllHairdressersServicesQuery, List<ServiceDto>>
{
    public async Task<List<ServiceDto>> Handle(GetAllHairdressersServicesQuery request, CancellationToken ct)
    {
        var hairdresserid = currentAppUser.User.Id;

        var services = await unitOfWork.ServiceRepository.GetAllHairdressersLookUpAsync(hairdresserid, ct);

        var servicesDto = mapper.Map<List<ServiceDto>>(services);

        logger.LogInformation("Hairdresser requested services. HairdresserId: {HairdresserId}, Records: {Count}", hairdresserid, services.Count);

        return servicesDto;
    }
}
