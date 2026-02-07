using BarberReservation.Shared.Models.Service;
using MediatR;

namespace BarberReservation.Application.Service.Queries.GetAllForHomePage;

public sealed class GetAllForHomePageQuery : IRequest<List<ServiceDto>> { }
