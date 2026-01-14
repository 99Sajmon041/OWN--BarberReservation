using BarberReservation.Application.HairdresserWorkingHours.Queries.Admin.GetWorkingHoursForHairdresser;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.HairdresserWorkingHours.Admin;
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
        public async Task<ActionResult<IReadOnlyList<AdminHairdresserWorkingHoursDto>>> GetAllByHairdresser(string hairdresserId, CancellationToken ct)
        {
            var result = await mediator.Send(new GetWorkingHoursForHairdresserQuery(hairdresserId), ct);
            return Ok(result);
        }
    }
}
