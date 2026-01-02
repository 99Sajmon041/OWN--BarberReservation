using MediatR;

namespace BarberReservation.Application.User.Commands.Admin.DeactivateUser;

public sealed class DeactivateUserCommand(string id) : IRequest
{
    public string Id { get; set; } = id;
}
