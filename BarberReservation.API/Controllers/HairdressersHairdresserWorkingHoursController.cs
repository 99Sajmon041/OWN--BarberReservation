using BarberReservation.Application.HairdresserWorkingHours.Queries.Hairdresser.GetSelfWorkingHours;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.HairdresserWorkingHours.Hairdresser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberReservation.API.Controllers
{
    [Route("api/me/working-hours")]
    [Authorize(Roles = nameof(UserRoles.Hairdresser))]
    [ApiController]
    public class HairdressersHairdresserWorkingHoursController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<HairdresserWorkingHoursDto>>> GetMyWorkingHours(CancellationToken ct)
        {
            var result = await mediator.Send(new GetSelfWorkingHoursQuery(), ct);
            return Ok(result);
        }

        //upsert
    }
}
