namespace BarberReservation.Application.UserIdentity;

public interface IUserContext
{
    CurrentUser? GetCurrentUser();
}
