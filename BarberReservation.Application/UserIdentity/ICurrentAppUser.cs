using BarberReservation.Domain.Entities;

namespace BarberReservation.Application.UserIdentity;

public interface ICurrentAppUser
{
    ApplicationUser User { get; set; }
}
