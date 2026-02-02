using AutoMapper;
using BarberReservation.Application.TimeOff.Mapping;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.TimeOff;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.TimeOff.Queries.Admin.GetAllAdminTimeOffs;

public sealed class GetAllAdminTimeOffsQueryHandler(
    ILogger<GetAllAdminTimeOffsQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetAllAdminTimeOffsQuery, PagedResult<HairdresserTimeOffDto>>
{
    public async Task<PagedResult<HairdresserTimeOffDto>> Handle(GetAllAdminTimeOffsQuery request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var adminHairdresserPagedRequest = request.ToAdminHairdresserPagedRequest();

        var (items, total) = await unitOfWork.HairdresserTimeOffRepository.GetAllPagedForAdminAsync(adminHairdresserPagedRequest, request.Year, request.Month, ct);

        var itemsDto = mapper.Map<List<HairdresserTimeOffDto>>(items);

        logger.LogInformation("Admin fetched filtered {ItemsCount} records of Time-off.", items.Count);

        return new PagedResult<HairdresserTimeOffDto>
        {
            Items = itemsDto, 
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItemsCount = total
        };
    }
}
