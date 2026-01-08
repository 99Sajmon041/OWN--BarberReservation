namespace BarberReservation.Infrastructure.Options;

public sealed class DefaultUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public List<string> Roles { get; set; } = [];
}
