using BarberReservation.Application.HairdresserService.Commands.Self.CreateSelfHairdresserService;
using BarberReservation.Shared.Models.HairdresserService;

namespace BarberReservation.API.Mappings;

public static class HairdresserServiceMapping
{
    public static CreateSelfHairdresserServiceCommand ToCreateHairdresserServiceSelfCommand(this CreateHairdresserServiceRequest request)
    {
        return new CreateSelfHairdresserServiceCommand
        {
            ServiceId = request.ServiceId,
            DurationMinutes = request.DurationMinutes,
            Price = request.Price,
        };
    }
}
