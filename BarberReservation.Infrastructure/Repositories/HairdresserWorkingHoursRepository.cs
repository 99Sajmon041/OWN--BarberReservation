using BarberReservation.Domain.Interfaces;
using BarberReservation.Infrastructure.Database;

namespace BarberReservation.Infrastructure.Repositories;

public sealed class HairdresserWorkingHoursRepository(BarberDbContext context) : IHairdresserWorkingHoursRepository
{
    private readonly BarberDbContext _context = context;
}
