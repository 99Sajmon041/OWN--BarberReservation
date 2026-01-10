using BarberReservation.API.Mappings;
using BarberReservation.Application.Reservation.Mapping;
using BarberReservation.Application.Reservation.Queries.Hairdresser.GetAllHairdresserReservation;
using BarberReservation.Application.Reservation.Queries.Hairdresser.GetHairDresserReservation;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Rezervation.Common;
using BarberReservation.Shared.Models.Rezervation.Hairdresser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberReservation.API.Controllers
{
    [Route("api/hairdresser/reservations")]
    [Authorize(Roles = nameof(UserRoles.Hairdresser))]
    [ApiController]
    public class HairdresserReservationController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedResult<HairdresserReservationDto>>> GetAll([FromQuery] GetAllHairdresserReservationQuery query, CancellationToken ct)
        {
            var result = await mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<HairdresserReservationDto>> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetHairDresserReservationQuery(id), ct);
            return Ok(result);
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateReservationRequest request, CancellationToken ct)
        {
            var command = request.ToHairDresserUpdateReservationCommand(id);
            await mediator.Send(command, ct);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateHairDresserReservationRequest request, CancellationToken ct)
        {
            var command = request.ToCreateHairDresserReservationCommand();
            var reservationId = await mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id = reservationId }, reservationId);
        }
    }
}
