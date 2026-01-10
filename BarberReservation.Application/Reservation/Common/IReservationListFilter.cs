using BarberReservation.Shared.Enums;

namespace BarberReservation.Application.Reservation.Common;

public interface IReservationListFilter
{
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
