using BarberReservation.Domain.Entities;

namespace BarberReservation.Application.Authorization.Jwt;

public interface IJwtTokenService
{
    Task<(string Token, DateTime ExpiresAt)> CreateTokenAsync(ApplicationUser user, bool rememberMe);
}
