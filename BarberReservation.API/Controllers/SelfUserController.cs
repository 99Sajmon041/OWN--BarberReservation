using BarberReservation.API.Mappings;
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

        [HttpPatch("deactivate")]
        public async Task<IActionResult> DeactivateProfile(CancellationToken ct)
        {
            await mediator.Send(new DeactivateAccountCommand(), ct);

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserRequest request, CancellationToken ct)
        {
            var command = request.ToUpdateAccountCommand();
            await mediator.Send(command, ct);

            return NoContent();
        }
    }
}
