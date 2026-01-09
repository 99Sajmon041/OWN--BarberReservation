using BarberReservation.Domain.Entities;

namespace BarberReservation.Application.UserIdentity;

public sealed class CurrentAppUser : ICurrentAppUser
{
    public ApplicationUser User { get; private set; } = default!;
    public bool IsInitialized { get; private set; }

    public void Initialize(ApplicationUser user)
    {
        if(IsInitialized)
            throw new InvalidOperationException("CurrentAppUser is already initialized.");

        User = user ?? throw new ArgumentNullException(nameof(user));
        IsInitialized = true;
    }
}
