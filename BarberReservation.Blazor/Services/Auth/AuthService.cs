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
    private readonly HttpClient http = factory.CreateClient("Api");

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var response = await http.PostAsJsonAsync("api/auth/login", request, ct);

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

        var response = await http.SendAsync(msg, ct);

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

    public async Task RegisterAsync(RegisterRequest register, CancellationToken ct)
    {
        var response = await http.PostAsJsonAsync("api/auth/register", register, ct);
        if(!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se dokončit registraci, opakujte akci později.");
            throw new ApiRequestException(message, (int)response.StatusCode);
        }
    }

    public async Task ForgottenPasswordAsync(ForgotPasswordRequest request, CancellationToken ct)
    {
        var response = await http.PostAsJsonAsync("api/auth/forgot-password", request, ct);
        if (!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Nepodařilo se odeslat e-mail, opakujte akci později.");
            throw new ApiRequestException(message, (int)response.StatusCode);
        }
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct)
    {
        var response = await http.PostAsJsonAsync("api/auth/reset-password", request, ct);
        if (!response.IsSuccessStatusCode)
        {
            var message = await response.ReadProblemMessageAsync("Heslo se nepodařilo obnovit, opakujte akci později.");
            throw new ApiRequestException(message, (int)response.StatusCode);
        }
    }
}
