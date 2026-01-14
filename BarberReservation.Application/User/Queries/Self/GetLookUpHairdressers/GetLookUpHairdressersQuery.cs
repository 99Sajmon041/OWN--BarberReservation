using MediatR;

namespace BarberReservation.Application.User.Queries.Self.GetLookUpHairdressers;

public sealed class GetLookUpHairdressersQuery : IRequest<IEnumerable<Shared.Models.LookUpModels.GetLookUpHairdressersByService>> { }
