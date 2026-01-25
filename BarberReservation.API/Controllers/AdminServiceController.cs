using BarberReservation.API.Mappings;
using BarberReservation.Application.Service.Commands.ActivateService;
using BarberReservation.Application.Service.Commands.DeactivateService;
using BarberReservation.Application.Service.Queries.GetAllServices;
using BarberReservation.Application.Service.Queries.GetServiceById;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Service;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberReservation.API.Controllers
{
    [Route("api/services")]
    [ApiController]
    [Authorize(Roles = nameof(UserRoles.Admin))]
    public class AdminServiceController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedResult<ServiceDto>>> GetAll([FromQuery] GetAllServicesQuery query, CancellationToken ct)
        {
            var result = await mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ServiceDto>> GetById([FromRoute] int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetServiceByIdQuery(id), ct);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromBody] UpsertServiceRequest request, [FromRoute] int id, CancellationToken ct)
        {
            var command = request.ToUpdateServiceCommand(id);
            await mediator.Send(command, ct);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<object>> Create([FromBody] UpsertServiceRequest request, CancellationToken ct)
        {
            var command = request.ToCreateServiceCommand();
            var id = await mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPatch("{id:int}/deactivate")]
        public async Task<IActionResult> Deactivate(int id, CancellationToken ct)
        {
            await mediator.Send(new DeactivateServiceCommand(id), ct);
            return NoContent();
        }

        [HttpPatch("{id:int}/activate")]
        public async Task<IActionResult> Activate(int id, CancellationToken ct)
        {
            await mediator.Send(new ActivateServiceCommand(id), ct);
            return NoContent();
        }
    }
}

