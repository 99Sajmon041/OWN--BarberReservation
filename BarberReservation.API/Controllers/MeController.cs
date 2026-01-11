using BarberReservation.Application.User.Queries.Self.GetLookUpCustomer;
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
        public async Task<ActionResult<ReservationClientLookUpDto>> GetProfile(CancellationToken ct)
        {
            var result = await mediator.Send(new GetLookUpCustomerQuery(), ct);
            return Ok(result);
        }
    }
}
