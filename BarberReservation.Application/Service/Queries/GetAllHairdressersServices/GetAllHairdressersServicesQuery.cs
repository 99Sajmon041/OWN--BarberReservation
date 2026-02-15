using BarberReservation.Application.Common.Security;
using BarberReservation.Shared.Models.Service;
using MediatR;

namespace BarberReservation.Application.Service.Queries.GetAllHairdressersServices;

public sealed class GetAllHairdressersServicesQuery : IRequireActiveUser, IRequest<List<ServiceDto>> { }
