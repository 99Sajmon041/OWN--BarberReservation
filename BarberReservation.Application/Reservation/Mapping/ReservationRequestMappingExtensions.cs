using BarberReservation.Application.Reservation.Queries.Admin.GetAllAdminReservations;
using BarberReservation.Application.Reservation.Queries.Hairdresser.GetAllHairdresserReservations;
using BarberReservation.Application.Reservation.Queries.Self.GetAllSelfReservations;
using BarberReservation.Shared.Models.Reservation;

namespace BarberReservation.Application.Reservation.Mapping;

public static class ReservationRequestMappingExtensions
{
    public static ReservationPagedRequest ToAdminReservationPagedRequest(this GetAllAdminReservationsQuery query)
    {
        return new ReservationPagedRequest
        {
            Page = query.Page,
            PageSize = query.PageSize,
            HairdresserId = query.HairdresserId,
            CustomerId = query.CustomerId,
            ServiceId = query.ServiceId,
            Status = query.Status,
            CanceledBy = query.CanceledBy,
            CanceledReason = query.CanceledReason,
            CreatedFrom = query.CreatedFrom,
            CreatedTo = query.CreatedTo,
            StartFrom = query.StartFrom,
            StartTo = query.StartTo,
            CanceledFrom = query.CanceledFrom,
            CanceledTo = query.CanceledTo,
            Search = query.Search,
            SortBy = query.SortBy,
            Desc = query.Desc
        };
    }

    public static ReservationPagedRequest ToHairDresserReservationPagedRequest(this GetAllHairdresserReservationsQuery query)
    {
        return new ReservationPagedRequest
        {
            Page = query.Page,
            PageSize = query.PageSize,
            ServiceId = query.ServiceId,
            Status = query.Status,
            CanceledBy = query.CanceledBy,
            CanceledReason = query.CanceledReason,
            CreatedFrom = query.CreatedFrom,
            CreatedTo = query.CreatedTo,
            StartFrom = query.StartFrom,
            StartTo = query.StartTo,
            CanceledFrom = query.CanceledFrom,
            CanceledTo = query.CanceledTo,
            Search = query.Search,
            SortBy = query.SortBy,
            Desc = query.Desc
        };
    }

    public static ReservationPagedRequest ToSelfReservationPagedRequest(this GetAllSelfReservationsQuery query)
    {
        return new ReservationPagedRequest
        {
            Page = query.Page,
            PageSize = query.PageSize,
            HairdresserId = query.HairdresserId,
            ServiceId = query.ServiceId,
            Status = query.Status,
            CanceledBy = query.CanceledBy,
            CanceledReason = query.CanceledReason,
            CreatedFrom = query.CreatedFrom,
            CreatedTo = query.CreatedTo,
            StartFrom = query.StartFrom,
            StartTo = query.StartTo,
            CanceledFrom = query.CanceledFrom,
            CanceledTo = query.CanceledTo,
            Search = query.Search,
            SortBy = query.SortBy,
            Desc = query.Desc
        };
    }
}
