namespace BarberReservation.Application.UserIdentity;

public sealed record CurrentUser(string Id, string Email, string[] Roles);
