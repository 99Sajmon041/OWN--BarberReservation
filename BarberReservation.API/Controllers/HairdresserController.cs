using BarberReservation.Application.HairdresserService.Queries.Self.GetAllSelfHairdressersServices;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.HairdresserService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberReservation.API.Controllers
{
    [Authorize(Roles = nameof(UserRoles.Hairdresser))]
    [Route("api/hairdresser-services")]
    [ApiController]
    public class HairdresserController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedResult<HairdresserServiceDto>>> GetAll([FromQuery] HairdresserSelfServicePagedRequest request, CancellationToken ct)
        {
            var query = new GetAllSelfHairdressersServicesQuery
            {
                Page = request.Page,
                PageSize = request.PageSize,
                ServiceId = request.ServiceId,
                Search = request.Search,
                IsActive = request.IsActive,
                SortBy = request.SortBy,
                Desc = request.Desc
            };

            var result = await mediator.Send(query, ct);
            return Ok(result);
        }
    }
}
