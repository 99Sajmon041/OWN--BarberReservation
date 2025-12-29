using BarberReservation.Application.User.Commands.Self.DeactivateAccount;
using BarberReservation.Application.User.Queries.Self.GetProfile;
using BarberReservation.Shared.Models.User.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberReservation.API.Controllers
{
    [Route("api/users/me")]
    [ApiController]
    [Authorize]
    public class SelfUserController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetProfile(CancellationToken ct)
        {
            var result = await mediator.Send(new GetProfileQuery(), ct);
            return Ok(result);
        }

        [HttpPost("deactivate")]
        public async Task<IActionResult> Deactivate(CancellationToken ct)
        {
            await mediator.Send(new DeactivateAccountCommand(), ct);
            return NoContent();
        }
    }
}
