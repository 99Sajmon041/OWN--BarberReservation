using BarberReservation.Application.HairdresserService.Commands.Admin.DeleteHairdresserService;
using BarberReservation.Application.HairdresserService.Commands.Admin.NewFolder;
using BarberReservation.Application.HairdresserService.Queries.Admin.GetAllHairdresserServices;
using BarberReservation.Application.HairdresserService.Queries.Admin.GetHairdresserService;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.HairdresserService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberReservation.API.Controllers
{
    [Authorize(Roles = nameof(UserRoles.Admin))]
    [Route("api/admin/hairdresser-services")]
    [ApiController]
    public class AdminHairdresserController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedResult<HairdresserServiceDto>>> GetAll([FromQuery] HairdresserAdminServicePagedRequest request, CancellationToken ct)
        {
            var query = new GetAllHairdressersServicesQuery
            {
                Page = request.Page,
                PageSize = request.PageSize,
                HairdresserId = request.HairdresserId,
                ServiceId = request.ServiceId,
                Search = request.Search,
                IsActive = request.IsActive,
                SortBy = request.SortBy,
                Desc = request.Desc
            };

            var result = await mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<HairdresserServiceDto>> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetHairdresserServiceByIdQueryQuery(id), ct);
            return Ok(result);
        }

        [HttpPatch("{id:int}/deactivate")]
        public async Task<IActionResult> Deactivate(int id, CancellationToken ct)
        {
            await mediator.Send(new DeactivateHairdresserServiceCommand(id), ct);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await mediator.Send(new DeleteHairdresserServiceCommand(id), ct);
            return NoContent();
        }
    }
}
    