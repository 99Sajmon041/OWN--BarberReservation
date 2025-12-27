using BarberReservation.API.Mappings;
using BarberReservation.Shared.Models.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberReservation.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public sealed class AuthController(IMediator mediator) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            var command = request.GetLoginCommand();
            var result = await mediator.Send(command, ct);

            return Ok(result);
        }
    }
}
