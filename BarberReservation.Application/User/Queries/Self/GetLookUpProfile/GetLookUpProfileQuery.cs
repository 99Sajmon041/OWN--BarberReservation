using BarberReservation.Application.Common.Security;
using BarberReservation.Shared.Models.LookUpModels;
using MediatR;

namespace BarberReservation.Application.User.Queries.Self.GetLookUpProfile;

public sealed class GetLookUpProfileQuery : IRequireActiveUser, IRequest<ReservationClientLookupDto> { }
