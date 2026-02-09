using BarberReservation.Shared.Enums;

namespace BarberReservation.Application.Reservation.Common.Interfaces;

public interface IReservationListFilter
{
    int? ServiceId { get; }
    string? HairdresserId { get; }
    ReservationStatus? Status { get; }
    ReservationCanceledBy? CanceledBy { get; }
    CanceledReason? CanceledReason { get; }
    DateTime? CreatedFrom { get; }
    DateTime? CreatedTo { get; }
    DateTime? StartFrom { get; }
    DateTime? StartTo { get; }
    DateTime? CanceledFrom { get; }
    DateTime? CanceledTo { get; }
}
