using BarberReservation.Shared.Models.Authorization;

namespace BarberReservation.Blazor.Services.Auth;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct);
    Task LogoutAsync();
    Task ChangePasswordAsync(ChangePasswordRequest request, CancellationToken ct);
    Task RegisterAsync(RegisterRequest register, CancellationToken ct);
    Task ForgottenPasswordAsync(ForgotPasswordRequest request, CancellationToken ct);
    Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct);
}
