using BarberReservation.API.Mappings;
using BarberReservation.Application.HairdresserService.Queries.Self.GetAllSelfHairdressersServices;
using BarberReservation.Application.HairdresserService.Queries.Self.GetSelfHairdressersService;
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<HairdresserServiceDto>> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetSelfHairdresserServiceByIdQuery(id), ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHairdresserServiceRequest request, CancellationToken ct)
        {
            var command = request.ToCreateHairdresserServiceSelfCommand();
            var id = await mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }
    }
}
