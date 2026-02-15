using BarberReservation.Application.HairdresserWorkingHours.Commands.Hairdresser.UpsertSelfWorkingHours;
using BarberReservation.Application.HairdresserWorkingHours.Mapping;
using BarberReservation.Application.HairdresserWorkingHours.Queries.Hairdresser.ExistsWorkingHoursByHairdresser;
using BarberReservation.Application.HairdresserWorkingHours.Queries.Hairdresser.GetNextSelfWorkingHours;
using BarberReservation.Application.HairdresserWorkingHours.Queries.Hairdresser.GetSelfDailyWorkingHours;
using BarberReservation.Application.HairdresserWorkingHours.Queries.Hairdresser.GetSelfWorkingHours;
using BarberReservation.Application.HairdresserWorkingHours.Queries.Hairdresser.GetSelfWorkingHoursByWeek;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.HairdresserWorkingHours;
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
        public async Task<ActionResult<HairdresserWorkingHoursDto>> GetMyWorkingHours(CancellationToken ct)
        {
            var result = await mediator.Send(new GetSelfWorkingHoursQuery(), ct);
            return Ok(result);
        }

        [HttpGet("upcoming")]
        public async Task<ActionResult<HairdresserWorkingHoursDto>> GetMyUpcomingWorkingHours(CancellationToken ct)
        {
            var result = await mediator.Send(new GetNextSelfWorkingHoursQuery(), ct);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWorkingDays([FromBody] HairdresserWorkingHoursUpsertDto request,  CancellationToken ct)
        {
            var daysOfWorkingWeek = request.ToUpsertHairdresserWorkingHoursDto();

            await mediator.Send(new UpsertSelfWorkingHoursCommand
            {
                DaysOfWorkingWeek = daysOfWorkingWeek
            }, ct);

            return NoContent();
        }

        [HttpGet("daily")]
        public async Task<ActionResult<WorkingHoursDto>> GetMyDailyWorkingHours([FromQuery] DateOnly day, CancellationToken ct)
        {
            var result = await mediator.Send(new GetSelfDailyWorkingHoursQuery(day), ct);
            return Ok(result);
        }

        [HttpGet("selected-week")]
        public async Task<ActionResult<List<WorkingHoursDto>>> GetSelectedWeekByHairdresser(DateOnly monday, CancellationToken ct)
        {
            var result = await mediator.Send(new GetSelfWorkingHoursByWeekQuery(monday), ct);
            return Ok(result);
        }

        [HttpGet("exists")]
        public async Task<ActionResult<bool>> ExistsWorkingHours(CancellationToken ct)
        {
            var result = await mediator.Send(new ExistsWorkingHoursByHairdresserQuery(), ct);
            return Ok(result);
        }
    }
}
