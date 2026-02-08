using BarberReservation.Application.Reservation.Commands.Admin.CreateAdminReservation;
using BarberReservation.Application.Reservation.Commands.Admin.UpdateAdminReservationStatuss;
using BarberReservation.Application.Reservation.Commands.Hairdresser.CreateHairDresserReservation;
using BarberReservation.Application.Reservation.Commands.Hairdresser.UpdateHairDresserReservationStatus;
using BarberReservation.Application.Reservation.Commands.Self.CreateSelfReservation;
using BarberReservation.Shared.Models.Reservation;

namespace BarberReservation.API.Mappings
{
    public static class ReservationMapping
    {
        public static UpdateAdminReservationStatusCommand ToAdminUpdateReservationCommand(this UpdateReservationStatusRequest request, int id)
        {
            return new UpdateAdminReservationStatusCommand
            {
                Id = id,
                NewReservationStatus = request.NewReservationStatus,
                CanceledReason = request.CanceledReason
            };
        }

        public static CreateAdminReservationCommand ToCreateAdminReservationCommand(this CreateReservationRequest request)
        {
            return new CreateAdminReservationCommand
            {
                HairdresserId = request.HairdresserId ?? "",
                HairdresserServiceId = request.HairdresserServiceId,
                StartAt = request.StartAt,
                CustomerId = request.CustomerId,
                CustomerName = request.CustomerName,
                CustomerEmail = request.CustomerEmail,
                CustomerPhone = request.CustomerPhone
            };
        }

        public static UpdateHairDresserReservationStatusCommand ToHairDresserUpdateReservationCommand(this UpdateReservationStatusRequest request, int id)
        {
            return new UpdateHairDresserReservationStatusCommand
            {
                Id = id,
                NewReservationStatus = request.NewReservationStatus,
                CanceledReason = request.CanceledReason
            };
        }

        public static CreateHairDresserReservationCommand ToCreateHairDresserReservationCommand(this CreateReservationRequest request)
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
        public static CreateSelfReservationCommand ToCreateSelfReservationCommand(this CreateReservationRequest request)
        {
            return new CreateSelfReservationCommand
            {
                HairdresserId = request.HairdresserId ?? "",
                HairdresserServiceId = request.HairdresserServiceId,
                StartAt = request.StartAt,
                CustomerName = request.CustomerName,
                CustomerEmail = request.CustomerEmail,
                CustomerPhone = request.CustomerPhone
            };
        }
    }
}
