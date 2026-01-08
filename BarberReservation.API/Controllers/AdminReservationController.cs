using BarberReservation.Application.Reservation.Queries.Admin.GetAllReservations;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Rezervation.Admin;
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
        public async Task<ActionResult<PagedResult<AdminReservationDto>>> GetAll([FromQuery] GetAllReservationsQuery query, CancellationToken ct)
        {
            var result = await mediator.Send(query, ct);
            return Ok(result);
        }
    }
}
