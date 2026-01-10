using BarberReservation.Application.Reservation.Commands.Self.SelfCancelReservation;
using BarberReservation.Application.Reservation.Queries.Self.GetAllSelfReservations;
using BarberReservation.Application.Reservation.Queries.Self.GetSelfReservation;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Reservation.Self;
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
        public async Task<ActionResult<PagedResult<SelfReservationDto>>> GetAll([FromQuery] GetAllSelfReservationsQuery query, CancellationToken ct)
        {
            var result = await mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = nameof(UserRoles.Customer))]
        public async Task<ActionResult<SelfReservationDto>> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetSelfReservationQuery(id), ct);
            return Ok(result);
        }

        [HttpPatch("{id:int}/cancel")]
        [Authorize(Roles = nameof(UserRoles.Customer))]
        public async Task<IActionResult> Cancel(int id, CancellationToken ct)
        {
            await mediator.Send(new SelfCancelReservationStatusCommand(id), ct);
            return NoContent();
        }
    }
}
