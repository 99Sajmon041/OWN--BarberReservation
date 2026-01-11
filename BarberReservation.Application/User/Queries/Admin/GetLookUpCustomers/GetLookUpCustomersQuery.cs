using BarberReservation.Application.Common.Validation.SearchValidation;
using BarberReservation.Shared.Models.LookUpModels;
using MediatR;

namespace BarberReservation.Application.User.Queries.Admin.GetLookUpCustomers;

public sealed class GetLookUpCustomersQuery : IHasSearch, IRequest<IEnumerable<ReservationClientLookUpDto>>
{
    public string Search { get; set; } = default!;
}
