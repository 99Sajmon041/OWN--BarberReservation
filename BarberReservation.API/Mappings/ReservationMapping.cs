using BarberReservation.Application.Reservation.Commands.Admin.CreateAdminReservation;
using BarberReservation.Application.Reservation.Commands.Admin.UpdateAdminReservationStatuss;
using BarberReservation.Application.Reservation.Commands.Hairdresser.UpdateHairDresserReservationStatus;
using BarberReservation.Shared.Models.Reservation.Admin;
using BarberReservation.Shared.Models.Reservation.Common;

namespace BarberReservation.API.Mappings
{
    public static class ReservationMapping
    {
        public static UpdateAdminReservationStatusCommand ToAdminUpdateReservationCommand(this UpdateReservationRequest request, int id)
        {
            return new UpdateAdminReservationStatusCommand
            {
                Id = id,
                NewReservationStatus = request.NewReservationStatus,
                CanceledReason = request.CanceledReason
            };
        }

        public static CreateAdminReservationCommand ToCreateAdminReservationCommand(this CreateAdminReservationRequest request)
        {
            return new CreateAdminReservationCommand
            {
                HairdresserId = request.HairdresserId,
                HairDresserServiceId = request.HairdresserServiceId,
                StartAt = request.StartAt,
                CustomerId = request.CustomerId,
                CustomerName = request.CustomerName,
                CustomerEmail = request.CustomerEmail,
                CustomerPhone = request.CustomerPhone
            };
        }

        public static UpdateHairDresserReservationStatusCommand ToHairDresserUpdateReservationCommand(this UpdateReservationRequest request, int id)
        {
            return new UpdateHairDresserReservationStatusCommand
            {
                Id = id,
                NewReservationStatus = request.NewReservationStatus,
                CanceledReason = request.CanceledReason
            };
        }
    }
}
