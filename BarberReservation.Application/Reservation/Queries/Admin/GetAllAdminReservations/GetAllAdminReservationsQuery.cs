using BarberReservation.Application.Common.PagedSettings;
using BarberReservation.Application.Reservation.Common;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Reservation.Admin;
using MediatR;

namespace BarberReservation.Application.Reservation.Queries.Admin.GetAllAdminReservations;

public sealed class GetAllAdminReservationsQuery : PagedApiRequest, IReservationListFilter, IRequest<PagedResult<AdminReservationDto>>
{
    public int? ServiceId { get; init; }
    public string? HairdresserId { get; init; }
    public ReservationStatus? Status { get; init; }
    public ReservationCanceledBy? CanceledBy { get; init; }
    public CanceledReason? CanceledReason { get; init; }
    public DateTime? CreatedFrom { get; init; }
    public DateTime? CreatedTo { get; init; }
    public DateTime? StartFrom { get; init; }
    public DateTime? StartTo { get; init; }
    public DateTime? CanceledFrom { get; init; }
    public DateTime? CanceledTo { get; init; }
}
