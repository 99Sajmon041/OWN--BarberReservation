using BarberReservation.Shared.Models.LookUpModels;
using MediatR;

namespace BarberReservation.Application.User.Queries.Self.GetLookUpHairdressers;

public sealed class GetLookUpHairdressersQuery : IRequest<IEnumerable<HairdresserLookUpDto>> { }
