using MediatR;

namespace BarberReservation.Application.User.Queries.Self.GetLookUpActiveHairdressers;

public sealed class GetLookUpActiveHairdressersQuery : IRequest<List<Shared.Models.LookUpModels.LookUpHairdressersDto>> { }
