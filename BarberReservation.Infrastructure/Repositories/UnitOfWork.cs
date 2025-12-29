using BarberReservation.Domain.Interfaces;
using BarberReservation.Infrastructure.Database;

namespace BarberReservation.Infrastructure.Repositories;

public sealed class UnitOfWork(BarberDbContext context) : IUnitOfWork
{
    private readonly BarberDbContext _context = context;

    private IServiceRepository? _serviceRepository;
    private IHairdresserServiceRepository? _hairdresserServiceRepository;
    private IReservationRepository? _reservationRepository;
    private IHairdresserTimeOffRepository? _hairdresserTimeOffRepository;
    private IHairdresserWorkingHoursRepository? _hairdresserWorkingHoursRepository;
    public IServiceRepository ServiceRepository => _serviceRepository ??= new ServiceRepository(_context);
    public IHairdresserServiceRepository HairdresserServiceRepository => _hairdresserServiceRepository ??= new HairdresserServiceRepository(_context);
    public IReservationRepository ReservationRepository => _reservationRepository ??= new ReservationRepository(_context);
    public IHairdresserTimeOffRepository HairdresserTimeOffRepository => _hairdresserTimeOffRepository ??= new HairdresserTimeOffRepository(_context);
    public IHairdresserWorkingHoursRepository HairdresserWorkingHoursRepository => _hairdresserWorkingHoursRepository ??= new HairdresserWorkingHoursRepository(_context);

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return _context.SaveChangesAsync(ct);
    }
}
