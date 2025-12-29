using BarberReservation.Domain.Interfaces;
using BarberReservation.Infrastructure.Database;

namespace BarberReservation.Infrastructure.Repositories;

public sealed class ServiceRepository(BarberDbContext context) : IServiceRepository
{
    private readonly BarberDbContext _context = context;
}
