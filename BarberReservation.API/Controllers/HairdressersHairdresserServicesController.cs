using BarberReservation.API.Mappings;
using BarberReservation.Application.HairdresserService.Commands.Self.DeactivateSelfHairdresserService;
using BarberReservation.Application.HairdresserService.Queries.Self.GetAllSelfHairdressersServices;
using BarberReservation.Application.HairdresserService.Queries.Self.GetSelfHairdressersService;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.HairdresserService.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberReservation.API.Controllers
{
    [Authorize(Roles = nameof(UserRoles.Hairdresser))]
    [Route("api/me/hairdresser-services")]
    [ApiController]
    public class HairdressersHairdresserServicesController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedResult<HairdresserServiceDto>>> GetAll([FromQuery] GetAllSelfHairdressersServicesQuery query, CancellationToken ct)
        {
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

        [HttpPatch("{id:int}/deactivate")]
        public async Task<IActionResult> Deactivate(int id, CancellationToken ct)
        {
            await mediator.Send(new DeactivateSelfHairdresserServiceCommand(id), ct);
            return NoContent();
        }
    }
}
