using MediatR;

namespace BarberReservation.Application.User.Commands.Admin.PartlyUpdateUser;

public sealed class PartlyUpdateUserCommand : IRequest
{
    public string Id { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
}
