using BarberReservation.Domain.Interfaces;
using BarberReservation.Infrastructure.Database;

namespace BarberReservation.Infrastructure.Repositories;

public sealed class ReservationRepository(BarberDbContext context) : IReservationRepository
{
    private readonly BarberDbContext _context = context;
}
