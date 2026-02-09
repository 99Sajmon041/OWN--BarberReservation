using BarberReservation.API.Mappings;
using BarberReservation.Application.Reservation.Commands.Self.SelfCancelReservation;
using BarberReservation.Application.Reservation.Common.GetAvailableSlotsForWeek;
using BarberReservation.Application.Reservation.Queries.Self.GetAllSelfReservations;
using BarberReservation.Application.Reservation.Queries.Self.GetSelfReservation;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Reservation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberReservation.API.Controllers
{
    [Route("api/me/reservations")]
    [ApiController]

    public class SelfReservationController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = nameof(UserRoles.Customer))]
        public async Task<ActionResult<PagedResult<ReservationDto>>> GetAll([FromQuery] GetAllSelfReservationsQuery query, CancellationToken ct)
        {
            var result = await mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = nameof(UserRoles.Customer))]
        public async Task<ActionResult<ReservationDto>> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetSelfReservationQuery(id), ct);
            return Ok(result);
        }

        [HttpPatch("{id:int}/cancel")]
        [Authorize(Roles = nameof(UserRoles.Customer))]
        public async Task<IActionResult> Cancel(int id, CancellationToken ct)
        {
            await mediator.Send(new SelfCancelReservationCommand(id), ct);
            return NoContent();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateReservationRequest request, CancellationToken ct)
        {
            var command = request.ToCreateSelfReservationCommand();
            await mediator.Send(command, ct);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("available-slots")]
        public async Task<ActionResult<IReadOnlyList<SlotDto>>> GetAvailableSlots(
            [FromQuery] string hairdresserId,
            [FromQuery] int serviceId,
            [FromQuery] DateTime weekStartDate, 
            CancellationToken ct)
        {
            var result = await mediator.Send(new GetAvailableSlotsForWeekQuery(hairdresserId, weekStartDate, serviceId), ct);
            return Ok(result);
        }
    }
}
