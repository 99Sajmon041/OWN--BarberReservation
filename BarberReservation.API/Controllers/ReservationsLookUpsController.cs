using BarberReservation.Application.ReservationEnums;
using BarberReservation.Application.Service.Queries.GetLookUpServices;
using BarberReservation.Application.User.Queries.Admin.GetLookUpCustomers;
using BarberReservation.Application.User.Queries.Self.GetLookUpHairdressers;
using BarberReservation.Application.User.Queries.Self.GetLookUpHairdressersByService;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.LookUpModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberReservation.API.Controllers
{
    [Route("api/reservations/lookups")]
    [ApiController]
    public class ReservationsLookUpsController(IMediator mediator) : ControllerBase
    {
        [HttpGet("customers")]
        [Authorize(Roles = nameof(UserRoles.Admin) +"," + nameof(UserRoles.Hairdresser))]
        public async Task<ActionResult<IEnumerable<ReservationClientLookUpDto>>> GetLookUpCustomers([FromQuery] string? search, CancellationToken ct)
        {
            var result = await mediator.Send(new GetLookUpCustomersQuery { Search = search ?? string.Empty }, ct);
            return Ok(result);
        }

        [HttpGet("hairdressers")]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<GetLookUpHairdressers>>> GetLookUpHairdressers(CancellationToken ct)
        {
            var result = await mediator.Send(new GetLookUpHairdressersQuery(), ct);
            return Ok(result);
        }

        [HttpGet("service/{serviceId:int}/hairdressers")]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<GetLookUpHairdressers>>> GetLookUpHaidressersByService(int serviceId, CancellationToken ct)
        {
            var result = await mediator.Send(new GetLookUpHairdressersByServiceQuery(serviceId), ct);
            return Ok(result);
        }

        [HttpGet("services")]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<ServiceLookUpDto>>> GetLookUpServices(CancellationToken ct)
        {
            var result = await mediator.Send(new GetLookUpServicesQuery(), ct);
            return Ok(result);
        }

        [HttpGet("reservation-canceled-reasons")]
        public async Task<ActionResult<IReadOnlyList<EnumLookUpDto>>> GetCanceledReasons(CancellationToken ct)
        {
            return Ok(await mediator.Send(new GetEnumLookUpQuery<CanceledReason>(), ct));
        }

        [HttpGet("reservation-canceled-by")]
        public async Task<ActionResult<IReadOnlyList<EnumLookUpDto>>> GetCanceledBy(CancellationToken ct)
        {
            return Ok(await mediator.Send(new GetEnumLookUpQuery<ReservationCanceledBy>(), ct));
        }

        [HttpGet("reservation-statuses")]
        public async Task<ActionResult<IReadOnlyList<EnumLookUpDto>>> GetReservationStatuses(CancellationToken ct)
        {
            return Ok(await mediator.Send(new GetEnumLookUpQuery<ReservationStatus>(), ct));
        }
    }
}