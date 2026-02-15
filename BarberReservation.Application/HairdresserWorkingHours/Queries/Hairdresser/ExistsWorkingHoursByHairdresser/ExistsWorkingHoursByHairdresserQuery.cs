using BarberReservation.Application.Common.Security;
using MediatR;

namespace BarberReservation.Application.HairdresserWorkingHours.Queries.Hairdresser.ExistsWorkingHoursByHairdresser;

public sealed class ExistsWorkingHoursByHairdresserQuery : IRequireActiveUser, IRequest<bool> { }
