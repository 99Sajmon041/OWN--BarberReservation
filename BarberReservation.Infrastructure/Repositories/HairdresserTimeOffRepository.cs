using BarberReservation.Domain.Interfaces;
using BarberReservation.Infrastructure.Database;

namespace BarberReservation.Infrastructure.Repositories;

public sealed class HairdresserTimeOffRepository(BarberDbContext context) : IHairdresserTimeOffRepository
{
    private readonly BarberDbContext _context = context;
}