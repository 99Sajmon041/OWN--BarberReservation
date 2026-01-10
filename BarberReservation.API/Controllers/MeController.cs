using BarberReservation.Application.User.Queries.Self.GetLookUpProfile;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.LookUpModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberReservation.API.Controllers
{
    [Route("api/me")]
    [ApiController]
    [Authorize(Roles = nameof(UserRoles.Customer))]
    public class MeController(IMediator mediator) : ControllerBase
    {
        [HttpGet("profile")]
        public async Task<ActionResult<ReservationClientLookupDto>> GetProfile(CancellationToken ct)
        {
            var result = await mediator.Send(new GetLookUpProfileQuery(), ct);
            return Ok(result);
        }
    }
}
