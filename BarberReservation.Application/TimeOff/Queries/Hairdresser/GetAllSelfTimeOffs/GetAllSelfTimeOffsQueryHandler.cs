using AutoMapper;
using BarberReservation.Application.TimeOff.Mapping;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.TimeOff.Hairdresser;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.TimeOff.Queries.Hairdresser.GetAllSelfTimeOffs;

public sealed class GetAllSelfTimeOffsQueryHandler(
    ILogger<GetAllSelfTimeOffsQueryHandler> logger,
    IUnitOfWork unitOfWork,
    ICurrentAppUser currentAppUser,
    IMapper mapper) : IRequestHandler<GetAllSelfTimeOffsQuery, PagedResult<HairdresserTimeOffDto>>
{
    public async Task<PagedResult<HairdresserTimeOffDto>> Handle(GetAllSelfTimeOffsQuery request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var hairdresser = currentAppUser.User;
        var hairdresserPagedRequest = request.ToHairdresserPagedRequest();

        var (items, total) = await unitOfWork.HairdresserTimeOffRepository.GetAllPagedForHairdresserAsync(
            hairdresser.Id,
            hairdresserPagedRequest,
            request.Year,
            request.Month, 
            ct);

        var dtos = mapper.Map<List<HairdresserTimeOffDto>>(items);

        logger.LogInformation("Hairdresser fetched filtered {ItemsCount} records of Time-off. Hairdresser ID: {HairdresserId}", items.Count, hairdresser.Id);

        return new PagedResult<HairdresserTimeOffDto>
        {
            Items = dtos,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItemsCount = total
        };
    }
}
