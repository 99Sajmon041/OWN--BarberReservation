using BarberReservation.Shared.Models.LookUpModels;
using MediatR;

namespace BarberReservation.Application.Service.Queries.GetLookUpServices;

public sealed class GetLookUpServicesQuery : IRequest<IReadOnlyList<ServiceLookUpDto>> { }
