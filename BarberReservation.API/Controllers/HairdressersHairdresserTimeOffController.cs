using BarberReservation.Application.TimeOff.Commands.Hairdresser.DeleteTimeOff;
using BarberReservation.Application.TimeOff.Mapping;
using BarberReservation.Application.TimeOff.Queries.Hairdresser.GetAllSelfTimeOffs;
using BarberReservation.Application.TimeOff.Queries.Hairdresser.GetSelfTimeOffsByDay;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.TimeOff.Common;
using BarberReservation.Shared.Models.TimeOff.Hairdresser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberReservation.API.Controllers
{
    [Route("api/me/timeoff")]
    [Authorize(Roles = nameof(UserRoles.Hairdresser))]
    [ApiController]
    public class HairdressersHairdresserTimeOffController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedResult<HairdresserTimeOffDto>>> GetAll([FromQuery] GetAllSelfTimeOffsQuery query, CancellationToken ct)
        {
            var result = await mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpGet("daily")]
        public async Task<ActionResult<List<HairdresserTimeOffDto>>> GetByDay([FromQuery] GetSelfTimeOffsByDayQuery query, CancellationToken ct)
        {
            var result = await mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UpsertTimeOffRequest request, CancellationToken ct)
        {
            var command = request.ToCreateSelfTimeOffCommand();
            await mediator.Send(command, ct);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpsertTimeOffRequest request, CancellationToken ct)
        {
            var command = request.ToUpdateSelfTimeOffCommand(id);
            await mediator.Send(command, ct);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await mediator.Send(new DeleteTimeOffCommand(id), ct);
            return NoContent();
        }
    }
}
