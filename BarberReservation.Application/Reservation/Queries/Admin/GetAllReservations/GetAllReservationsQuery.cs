using BarberReservation.Application.Common.PagedSettings;
using BarberReservation.Application.Common.Validation.SearchValidation;
using BarberReservation.Shared.Enums;
using BarberReservation.Shared.Models.Common;
using BarberReservation.Shared.Models.Rezervation.Admin;
using MediatR;

namespace BarberReservation.Application.Reservation.Queries.Admin.GetAllReservations;

public sealed class GetAllReservationsQuery : PagedApiRequest, IHasSearch, IRequest<PagedResult<AdminReservationDto>>
{
    public string? HairdresserId { get; set; }
    public ReservationStatus? Status { get; set; }
    public ReservationCanceledBy? CanceledBy { get; set; }
    public CanceledReason? CanceledReason { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
    public DateTime? StartFrom { get; set; }
    public DateTime? StartTo { get; set; }
    public DateTime? CanceledFrom { get; set; }
    public DateTime? CanceledTo { get; set; }
    public string? Search { get; set; }
    public string? SortBy { get; set; }
    public bool Desc { get; set; } = false;
}
