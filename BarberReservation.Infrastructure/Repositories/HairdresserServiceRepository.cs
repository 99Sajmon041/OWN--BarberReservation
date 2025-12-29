using BarberReservation.Domain.Interfaces;
using BarberReservation.Infrastructure.Database;

namespace BarberReservation.Infrastructure.Repositories;

public sealed class HairdresserServiceRepository(BarberDbContext context) : IHairdresserServiceRepository
{
    private readonly BarberDbContext _context = context;
}
