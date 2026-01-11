using BarberReservation.Application.Common.Security;
using BarberReservation.Shared.Models.LookUpModels;
using MediatR;

namespace BarberReservation.Application.User.Queries.Self.GetLookUpCustomer;

public sealed class GetLookUpCustomerQuery : IRequireActiveUser, IRequest<ReservationClientLookUpDto> { }
