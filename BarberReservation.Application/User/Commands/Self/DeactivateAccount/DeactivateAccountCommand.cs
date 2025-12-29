using BarberReservation.Application.Common.Security;
using MediatR;

namespace BarberReservation.Application.User.Commands.Self.DeactivateAccount;

public sealed class DeactivateAccountCommand : IRequest, IRequireUser
{
}
