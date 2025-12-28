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

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken ct)
        {
            var command = request.GetChangePasswordCommand();
            await mediator.Send(command, ct);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
        {
            var command = request.GetRegisterCommand();
            await mediator.Send(command, ct);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken ct)
        {
            var command = request.GetForgotPasswordCommand();
            await mediator.Send(command, ct);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken ct)
        {
            var command = request.GetResetPasswordCommand();
            await mediator.Send(command, ct);
            return NoContent();
        }
    }
}