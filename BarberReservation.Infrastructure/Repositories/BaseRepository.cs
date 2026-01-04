using BarberReservation.Domain.Interfaces;

namespace BarberReservation.Infrastructure.Repositories;

public abstract class BaseRepository
{
    protected static bool TryDeactivate(IActivatable entity)
    {
        if (!entity.IsActive)
            return false;

        entity.IsActive = false;
        return true;
    }
}
