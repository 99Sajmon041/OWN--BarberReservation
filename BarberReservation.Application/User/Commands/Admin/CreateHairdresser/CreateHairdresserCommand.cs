using MediatR;

namespace BarberReservation.Application.User.Commands.Admin.CreateHairdresser;

public sealed class CreateHairdresserCommand : IRequest
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}
