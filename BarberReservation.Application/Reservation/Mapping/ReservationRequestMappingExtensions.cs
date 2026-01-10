using BarberReservation.Application.Reservation.Commands.Hairdresser.CreateHairDresserReservation;
using BarberReservation.Application.Reservation.Queries.Admin.GetAllAdminReservations;
using BarberReservation.Application.Reservation.Queries.Hairdresser.GetAllHairdresserReservation;
using BarberReservation.Shared.Models.Rezervation.Common;
using BarberReservation.Shared.Models.Rezervation.Hairdresser;

namespace BarberReservation.Application.Reservation.Mapping;

public static class ReservationRequestMappingExtensions
{
    public static AdminReservationPagedRequest ToAdminReservationPagedRequest(this GetAllAdminReservationsQuery request)
    {
        return new AdminReservationPagedRequest
        {
            Page = request.Page,
            PageSize = request.PageSize,
            HairdresserId = request.HairdresserId,
            Status = request.Status,
            CanceledBy = request.CanceledBy,
            CanceledReason = request.CanceledReason,
            CreatedFrom = request.CreatedFrom,
            CreatedTo = request.CreatedTo,
            StartFrom = request.StartFrom,
            StartTo = request.StartTo,
            CanceledFrom = request.CanceledFrom,
            CanceledTo = request.CanceledTo,
            Search = request.Search,
            SortBy = request.SortBy,
            Desc = request.Desc
        };
    }

    public static HairdresserReservationPagedRequest ToHairDresserReservationPagedRequest(this GetAllHairdresserReservationQuery request)
    {
        return new HairdresserReservationPagedRequest
        {
            Page = request.Page,
            PageSize = request.PageSize,
            Status = request.Status,
            CanceledBy = request.CanceledBy,
            CanceledReason = request.CanceledReason,
            CreatedFrom = request.CreatedFrom,
            CreatedTo = request.CreatedTo,
            StartFrom = request.StartFrom,
            StartTo = request.StartTo,
            CanceledFrom = request.CanceledFrom,
            CanceledTo = request.CanceledTo,
            Search = request.Search,
            SortBy = request.SortBy,
            Desc = request.Desc
        };
    }

    public static CreateHairDresserReservationCommand ToCreateHairDresserReservationCommand(this CreateHairDresserReservationRequest request)
    {
        return new CreateHairDresserReservationCommand
        {
            HairDresserServiceId = request.HairdresserServiceId,
            StartAt = request.StartAt,
            CustomerId = request.CustomerId,
            CustomerName = request.CustomerName,
            CustomerEmail = request.CustomerEmail,
            CustomerPhone = request.CustomerPhone
        };
    }
}
