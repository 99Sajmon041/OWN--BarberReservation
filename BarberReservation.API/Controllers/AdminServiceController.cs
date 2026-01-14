using BarberReservation.API.Mappings;
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
    public class AdminServiceController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Hairdresser))]
        public async Task<ActionResult<PagedResult<ServiceDto>>> GetAll([FromQuery] GetAllServicesQuery query, CancellationToken ct)
        {
            var result = await mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Hairdresser))]
        public async Task<ActionResult<ServiceDto>> GetById([FromRoute] int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetServiceByIdQuery(id), ct);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = nameof(UserRoles.Admin))]
        public async Task<IActionResult> Update([FromBody] UpsertServiceRequest request, [FromRoute] int id, CancellationToken ct)
        {
            var command = request.ToUpdateServiceCommand(id);
            await mediator.Send(command, ct);
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRoles.Admin))]
        public async Task<ActionResult<object>> Create([FromBody] UpsertServiceRequest request, CancellationToken ct)
        {
            var command = request.ToCreateServiceCommand();
            var id = await mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPatch("{id:int}/deactivate")]
        [Authorize(Roles = nameof(UserRoles.Admin))]
        public async Task<IActionResult> Deactivate(int id, CancellationToken ct)
        {
            await mediator.Send(new DeactivateServiceCommand(id), ct);
            return NoContent();
        }
    }
}

