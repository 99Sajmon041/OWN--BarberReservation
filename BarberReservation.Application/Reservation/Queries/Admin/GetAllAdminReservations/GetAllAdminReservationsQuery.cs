
using BarberReservation.Application.Common.PagedSettings;
using BarberReservation.Application.Reservation.Common.Interfaces;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Reservation;
using MediatR;

namespace BarberReservation.Application.Reservation.Queries.Admin.GetAllAdminReservations;

public sealed class GetAllAdminReservationsQuery : PagedApiRequest, IReservationListFilter, IRequest<PagedResult<ReservationDto>>
{
    public int? ServiceId { get; init; }
    public string? HairdresserId { get; init; }
    public string? CustomerId { get; init; }
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
