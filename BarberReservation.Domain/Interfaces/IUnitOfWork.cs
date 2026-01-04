namespace BarberReservation.Domain.Interfaces;

public interface IUnitOfWork
{
    IServiceRepository ServiceRepository { get; }
    IHairdresserServiceRepository HairdresserServiceRepository { get; }
    IReservationRepository ReservationRepository { get; }
    IHairdresserTimeOffRepository HairdresserTimeOffRepository { get; }
    IHairdresserWorkingHoursRepository HairdresserWorkingHoursRepository { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
