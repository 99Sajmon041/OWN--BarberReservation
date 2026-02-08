using BarberReservation.Blazor.Auth;
using BarberReservation.Blazor.Common;
using BarberReservation.Blazor.Services.Auth;
using BarberReservation.Shared.Models.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

public sealed class AuthService(
    AuthenticationStateProvider authStateProvider,
    AuthState authState,
    IApiClient api) : IAuthService
{

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var req = new LoginRequest
        {
            Email = request.Email,
            Password = request.Password,
            RememberMe = true
        };

        var result = await api.PostAsyncWithResponse<LoginRequest, LoginResponse>(HttpMethod.Post, "api/auth/login", req, ct);

        if (string.IsNullOrWhiteSpace(result.Token))
            throw new ApiRequestException("Server nevrátil token.", 500);

        await authState.SetSessionAsync(result.Token, result.ExpiresAt, result.MustChangePassword);
        (authStateProvider as ApiAuthenticationStateProvider)?.NotifyUserChanged();

        return result;
    }

    public async Task ChangePasswordAsync(ChangePasswordRequest request, CancellationToken ct)
    {
        await api.SendAsync(HttpMethod.Post, "api/auth/change-password", request, ct);

        await authState.MarkPasswordChangedAsync();
        (authStateProvider as ApiAuthenticationStateProvider)?.NotifyUserChanged();
    }

    public async Task LogoutAsync()
    {
        await authState.ClearAsync();
        (authStateProvider as ApiAuthenticationStateProvider)?.NotifyUserChanged();
    }

    public async Task RegisterAsync(RegisterRequest request, CancellationToken ct)
    {
        await api.SendAsync(HttpMethod.Post, "api/auth/register", request, ct);
    }

    public async Task ForgottenPasswordAsync(ForgotPasswordRequest request, CancellationToken ct)
    {
        await api.SendAsync(HttpMethod.Post, "api/auth/forgot-password", request, ct);
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct)
    {
        await api.SendAsync(HttpMethod.Post, "api/auth/reset-password", request, ct);
    }
}
