using BarberReservation.Application.Reservation.Commands.Hairdresser.CreateHairDresserReservation;
using BarberReservation.Application.Reservation.Commands.Self.CreateSelfReservation;
using BarberReservation.Application.Reservation.Queries.Admin.GetAllAdminReservations;
using BarberReservation.Application.Reservation.Queries.Hairdresser.GetAllHairdresserReservations;
using BarberReservation.Application.Reservation.Queries.Self.GetAllSelfReservations;
using BarberReservation.Shared.Models.Reservation.Common;
using BarberReservation.Shared.Models.Reservation.Hairdresser;
using BarberReservation.Shared.Models.Reservation.Self;

namespace BarberReservation.Application.Reservation.Mapping;

public static class ReservationRequestMappingExtensions
{
    public static AdminReservationPagedRequest ToAdminReservationPagedRequest(this GetAllAdminReservationsQuery query)
    {
        return new AdminReservationPagedRequest
        {
            Page = query.Page,
            PageSize = query.PageSize,
            HairdresserId = query.HairdresserId,
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

    public static HairdresserReservationPagedRequest ToHairDresserReservationPagedRequest(this GetAllHairdresserReservationsQuery query)
    {
        return new HairdresserReservationPagedRequest
        {
            Page = query.Page,
            PageSize = query.PageSize,
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

    public static CreateHairDresserReservationCommand ToCreateHairDresserReservationCommand(this CreateHairDresserReservationRequest request)
    {
        return new CreateHairDresserReservationCommand
        {
            HairdresserServiceId = request.HairdresserServiceId,
            StartAt = request.StartAt,
            CustomerId = request.CustomerId,
            CustomerName = request.CustomerName,
            CustomerEmail = request.CustomerEmail,
            CustomerPhone = request.CustomerPhone
        };
    }

    public static SelfReservationPagedRequest ToSelfReservationPagedRequest(this GetAllSelfReservationsQuery query)
    {
        return new SelfReservationPagedRequest
        {
            Page = query.Page,
            PageSize = query.PageSize,
            HairdresserId = query.HairdresserId,
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

    public static CreateSelfReservationCommand ToCreateSelfReservationCommand(this CreateSelfReservationRequest request)
    {
        return new CreateSelfReservationCommand
        {
            HairdresserId = request.HairdresserId,
            HairdresserServiceId = request.HairdresserServiceId,
            StartAt = request.StartAt,
            CustomerName = request.CustomerName,
            CustomerEmail = request.CustomerEmail,
            CustomerPhone = request.CustomerPhone
        };
    }
}
