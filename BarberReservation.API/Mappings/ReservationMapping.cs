using BarberReservation.Application.Reservation.Commands.Admin.CreateReservation;
using BarberReservation.Application.Reservation.Commands.Admin.UpdateReservationStatus;
using BarberReservation.Shared.Models.Rezervation.Admin;

namespace BarberReservation.API.Mappings
{
    public static class ReservationMapping
    {
        public static UpdateReservationStatusCommand ToAdminUpdateReservationCommand(this UpdateReservationRequest request, int id)
        {
            return new UpdateReservationStatusCommand
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
                HairdresserServiceId = request.HairdresserServiceId,
                StartAt = request.StartAt,
                CustomerId = request.CustomerId,
                CustomerName = request.CustomerName,
                CustomerEmail = request.CustomerEmail,
                CustomerPhone = request.CustomerPhone
            };
        }
    }
}
