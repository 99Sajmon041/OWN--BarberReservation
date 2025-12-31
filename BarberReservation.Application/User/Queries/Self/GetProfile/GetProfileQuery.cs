using BarberReservation.Application.Common.Security;
using BarberReservation.Shared.Models.User.Common;
using MediatR;

namespace BarberReservation.Application.User.Queries.Self.GetProfile;

public sealed class GetProfileQuery : IRequest<UserDto>, IRequireActiveUser { }
