using BarberReservation.Application.UserIdentity;
using System.Security.Claims;

namespace BarberReservation.API.UserIdentity;

public sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUser? GetCurrentUser()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user?.Identity is null || !user.Identity.IsAuthenticated)
            return null;

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = user.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(email))
            return null;

        var roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();

        if (roles.Length == 0)
            return null;

        return new CurrentUser(userId, email, roles);
    }
}
