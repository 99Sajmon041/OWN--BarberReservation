using BarberReservation.API.Mappings;
using BarberReservation.Application.Reservation.Commands.Admin.ChangeReservationHairdresser;
using BarberReservation.Application.Reservation.Queries.Admin.GetAdminReservation;
using BarberReservation.Application.Reservation.Queries.Admin.GetAllAdminReservations;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Reservation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberReservation.API.Controllers
{
    [ApiController]
    [Route("api/admin/reservations")]
    [Authorize(Roles = nameof(UserRoles.Admin))]
    public class AdminReservationController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedResult<ReservationDto>>> GetAll([FromQuery] GetAllAdminReservationsQuery query, CancellationToken ct)
        {
            var result = await mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReservationDto>> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetAdminReservationQuery(id), ct);
            return Ok(result);
        }

        [HttpPut("{reservationId:int}/hairdresser")]
        public async Task<IActionResult> ChangeReservationHairdresser([FromRoute] int reservationId, [FromQuery] string hairdresserId, CancellationToken ct)
        {
            await mediator.Send(new ChangeReservationHairdresserCommand(reservationId, hairdresserId), ct);
            return NoContent();
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateReservationStatusRequest request, CancellationToken ct)
        {
            var command = request.ToAdminUpdateReservationCommand(id);
            await mediator.Send(command, ct);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateReservationRequest request, CancellationToken ct)
        {
            var command = request.ToCreateAdminReservationCommand();
            var reservationId = await mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id = reservationId }, reservationId);
        }
    }
}
