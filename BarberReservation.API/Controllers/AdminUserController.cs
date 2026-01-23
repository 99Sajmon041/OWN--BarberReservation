using BarberReservation.API.Mappings;
using BarberReservation.Application.User.Commands.Admin.DeactivateUser;
using BarberReservation.Application.User.Queries.Admin.GetAllUsers;
using BarberReservation.Application.User.Queries.Admin.GetUserById;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.User.Admin;
using BarberReservation.Shared.Models.User.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberReservation.API.Controllers
{
    [Authorize(Roles = nameof(UserRoles.Admin))]
    [Route("api/users")]
    [ApiController]
    public class AdminUserController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedResult<UserDto>>> GetAllUsers([FromQuery] GetAllUsersQuery query, CancellationToken ct)
        {
            var result = await mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserbyId([FromRoute] string id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetUserByIdQuery(id), ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken ct)
        {
            var command = request.ToCreateAccountCommand();
            await mediator.Send(command, ct);                  
            return NoContent();
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> DeactivateAccount([FromRoute] string id, CancellationToken ct)
        {
            await mediator.Send(new DeactivateUserCommand(id), ct);
            return NoContent();
        }

        [HttpPatch("{id}/update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request, [FromRoute] string id, CancellationToken ct)
        {
            var commad = request.ToUpdateUserCommand(id);
            await mediator.Send(commad, ct);
            return NoContent();
        }

        [HttpPatch("{id}/update-email")]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateUserEmailRequest request, [FromRoute] string id, CancellationToken ct)
        {
            var command = request.ToUpdateUserEmailCommnad(id);
            await mediator.Send(command, ct);
            return NoContent();
        }
    }
}
