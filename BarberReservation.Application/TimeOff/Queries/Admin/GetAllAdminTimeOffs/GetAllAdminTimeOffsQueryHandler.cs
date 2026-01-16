using AutoMapper;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.TimeOff.Admin;
using BarberReservation.Application.TimeOff.Mapping;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.TimeOff.Queries.Admin.GetAllAdminTimeOffs;

public sealed class GetAllAdminTimeOffsQueryHandler(
    ILogger<GetAllAdminTimeOffsQueryHandler> logger,
    IUnitOfWork unitOfwork,
    IMapper mapper) : IRequestHandler<GetAllAdminTimeOffsQuery, PagedResult<AdminHairdresserTimeOffDto>>
{
    public async Task<PagedResult<AdminHairdresserTimeOffDto>> Handle(GetAllAdminTimeOffsQuery request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var adminHairdresserPagedRequest = request.ToAdminHairdresserPagedRequest();

        var (items, total) = await unitOfwork.HairdresserTimeOffRepository.GetAllPagedForAdminAsync(adminHairdresserPagedRequest, request.Year, request.Month, ct);

        var itemsDto = mapper.Map<List<AdminHairdresserTimeOffDto>>(items);

        logger.LogInformation("Admin fetched filtered {ItemsCount} records of Time-off.", items.Count);

        return new PagedResult<AdminHairdresserTimeOffDto>
        {
            Items = itemsDto, 
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItemsCount = total
        };
    }
}
