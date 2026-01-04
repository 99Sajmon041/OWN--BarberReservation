using BarberReservation.Application.Common.Security;
using MediatR;

namespace BarberReservation.Application.HairdresserService.Commands.Self.CreateSelfHairdresserService;

public sealed class CreateSelfHairdresserServiceCommand : IRequest<int>, IRequireActiveUser
{
    public int ServiceId { get; set; }
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
}
