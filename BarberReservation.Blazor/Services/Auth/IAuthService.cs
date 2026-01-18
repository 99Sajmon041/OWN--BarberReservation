using BarberReservation.Shared.Models.Authorization;

namespace BarberReservation.Blazor.Services.Auth;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct);
    Task LogoutAsync();
    Task ChangePasswordAsync(ChangePasswordRequest request, CancellationToken ct);
}
