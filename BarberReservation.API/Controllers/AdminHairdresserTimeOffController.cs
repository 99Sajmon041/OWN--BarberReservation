using BarberReservation.Application.TimeOff.Queries.Admin.GetAllAdminTimeOffs;
using BarberReservation.Application.TimeOff.Queries.Admin.GetAllAdminTimeOffsWeekly;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.TimeOff;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberReservation.API.Controllers
{
    [Route("api/admin/time-off")]
    [Authorize(Roles = nameof(UserRoles.Admin))]
    [ApiController]
    public class AdminHairdresserTimeOffController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedResult<HairdresserTimeOffDto>>> GetAll([FromQuery] GetAllAdminTimeOffsQuery query, CancellationToken ct)
        {
            var result = await mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpGet("weekly")]
        public async Task<ActionResult<List<HairdresserTimeOffDto>>> GetAllWeekly([FromQuery] string hairdresserId, [FromQuery] DateTime weekStartDate, CancellationToken ct)
        {
            var result = await mediator.Send(new GetAllAdminTimeOffsWeeklyQuery(weekStartDate, hairdresserId), ct);
            return Ok(result);
        }
    }
}
