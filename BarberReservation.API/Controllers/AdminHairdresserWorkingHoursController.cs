using BarberReservation.Application.HairdresserWorkingHours.Queries.Admin.GetNextWorkingHoursForHairdresser;
using BarberReservation.Application.HairdresserWorkingHours.Queries.Admin.GetWorkingHoursForHairdresser;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.HairdresserWorkingHours.Hairdresser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberReservation.API.Controllers
{
    [Route("api/admin/working-hours")]
    [Authorize(Roles = nameof(UserRoles.Admin))]
    [ApiController]
    public class AdminHairdresserWorkingHoursController(IMediator mediator) : ControllerBase
    {
        [HttpGet("{hairdresserId}")]
        public async Task<ActionResult<HairdresserWorkingHoursDto>> GetAllByHairdresser(string hairdresserId, CancellationToken ct)
        {
            var result = await mediator.Send(new GetWorkingHoursForHairdresserQuery(hairdresserId), ct);
            return Ok(result);
        }

        [HttpGet("{hairdresserId}/upcoming")]
        public async Task<ActionResult<HairdresserWorkingHoursDto>> GetAllUpcomingByHairdresser(string hairdresserId, CancellationToken ct)
        {
            var result = await mediator.Send(new GetNextWorkingHoursForHairdresserQuery(hairdresserId), ct);
            return Ok(result);
        }
    }
}
