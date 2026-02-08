using BarberReservation.API.Mappings;
using BarberReservation.Application.Reservation.Queries.Hairdresser.GetAllHairdresserReservations;
using BarberReservation.Application.Reservation.Queries.Hairdresser.GetHairDresserReservation;
using BarberReservation.Application.Reservation.Queries.Hairdresser.GetHairdresserReservationByDay;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Reservation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberReservation.API.Controllers
{
    [Route("api/hairdresser/reservations")]
    [Authorize(Roles = nameof(UserRoles.Hairdresser))]
    [ApiController]
    public class HairdressersHairdresserReservationController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedResult<ReservationDto>>> GetAll([FromQuery] GetAllHairdresserReservationsQuery query, CancellationToken ct)
        {
            var result = await mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReservationDto>> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetHairDresserReservationQuery(id), ct);
            return Ok(result);
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateReservationStatusRequest request, CancellationToken ct)
        {
            var command = request.ToHairDresserUpdateReservationCommand(id);
            await mediator.Send(command, ct);
            return NoContent();
        }

        [HttpGet("daily")]
        public async Task<ActionResult<List<ReservationDto>>> GetMyDailyReservations([FromQuery]DateOnly day, CancellationToken ct)
        {
            var result = await mediator.Send(new GetHairdresserReservationByDayQuery(day), ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateReservationRequest request, CancellationToken ct)
        {
            var command = request.ToCreateHairDresserReservationCommand();
            var reservationId = await mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id = reservationId }, reservationId);
        }
    }
}
