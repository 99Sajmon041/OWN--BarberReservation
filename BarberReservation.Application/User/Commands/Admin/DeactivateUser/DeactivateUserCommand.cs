using BarberReservation.Application.Common.Security;
using MediatR;

namespace BarberReservation.Application.User.Commands.Admin.DeactivateUser;

public sealed class DeactivateUserCommand(string id) : IRequireActiveUser, IRequest
{
    public string Id { get; set; } = id;
}
