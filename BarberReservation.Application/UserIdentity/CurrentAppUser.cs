using BarberReservation.Domain.Entities;

namespace BarberReservation.Application.UserIdentity;

public sealed class CurrentAppUser : ICurrentAppUser
{
    public ApplicationUser User { get; set; } = default!;
}
