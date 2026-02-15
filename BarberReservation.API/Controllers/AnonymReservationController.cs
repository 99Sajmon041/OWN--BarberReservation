using BarberReservation.API.Mappings;
using BarberReservation.Shared.Models.Reservation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberReservation.API.Controllers
{
    [Route("api/anonymous/reservations")]
    [ApiController]
    [AllowAnonymous]
    public class AnonymReservationController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationRequest request, CancellationToken ct)
        {
            var command = request.ToCreateAnonymReservationCommand();
            await mediator.Send(command, ct);
            return NoContent();
        }
    }
}
