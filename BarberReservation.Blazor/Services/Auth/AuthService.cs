using BarberReservation.Blazor.Auth;
using BarberReservation.Blazor.Common;
using BarberReservation.Blazor.Services.Auth;
using BarberReservation.Blazor.Utils;
using BarberReservation.Shared.Models.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;

public sealed class AuthService(
    IHttpClientFactory factory,
    AuthenticationStateProvider authStateProvider,
    AuthState authState) : IAuthService
{
    private readonly HttpClient _http = factory.CreateClient("Api");

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", request, ct);

        if (!response.IsSuccessStatusCode)
        {
            var msg = await response.ReadProblemMessageAsync("Došlo k neočekávané chybě při přihlášení.");
            throw new ApiRequestException(msg, (int)response.StatusCode);
        }

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>(ct)
            ?? throw new ApiRequestException("Server nevrátil platnou odpověď.", (int)response.StatusCode);

        if (string.IsNullOrWhiteSpace(result.Token))
            throw new ApiRequestException("Server nevrátil token.", (int)response.StatusCode);

        await authState.SetSessionAsync(result.Token, result.ExpiresAt, result.MustChangePassword, request.RememberMe);
        (authStateProvider as ApiAuthenticationStateProvider)?.NotifyUserChanged();

        return result;
    }

    public async Task ChangePasswordAsync(ChangePasswordRequest request, CancellationToken ct)
    {
        var token = authState.Token;
        if (string.IsNullOrWhiteSpace(token))
            throw new ApiRequestException("Chybí přihlašovací token. Přihlas se prosím znovu.", 401);

        using var msg = new HttpRequestMessage(HttpMethod.Post, "api/auth/change-password")
        {
            Content = JsonContent.Create(request)
        };
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.SendAsync(msg, ct);

        if (!response.IsSuccessStatusCode)
        {
            var text = await response.ReadProblemMessageAsync("Změna hesla se nezdařila.");
            throw new ApiRequestException(text, (int)response.StatusCode);
        }

        await authState.MarkPasswordChangedAsync();
        (authStateProvider as ApiAuthenticationStateProvider)?.NotifyUserChanged();
    }

    public async Task LogoutAsync()
    {
        await authState.ClearAsync();
        (authStateProvider as ApiAuthenticationStateProvider)?.NotifyUserChanged();
    }
}
