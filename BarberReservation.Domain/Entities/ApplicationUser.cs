using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberReservation.Domain.Entities;

public sealed class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public bool MustChangePassword { get; set; } = false;
    public bool IsActive { get; set; }

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
    public ICollection<HairdresserTimeOff> TimeOffs { get; set; } = [];
    public ICollection<HairdresserWorkingHours> WorkingHours { get; set; } = [];
    public ICollection<Reservation> HairdresserReservations { get; set; } = [];
    public ICollection<Reservation> CustomerReservations { get; set; } = [];
    public ICollection<HairdresserService> HairdresserServices { get; set; } = [];
}
