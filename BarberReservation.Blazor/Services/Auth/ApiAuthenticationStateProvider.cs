using BarberReservation.Blazor.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BarberReservation.Blazor.Services.Auth;

public sealed class ApiAuthenticationStateProvider(AuthState authState) : AuthenticationStateProvider
{
    private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (!authState.IsAuthenticated)
            return Task.FromResult(new AuthenticationState(Anonymous));

        var claims = JwtClaimsParser.ParseClaimsFromJwt(authState.Token!);
        var identity = new ClaimsIdentity(claims, "jwt");
        var principal = new ClaimsPrincipal(identity);

        return Task.FromResult(new AuthenticationState(principal));
    }

    public void NotifyUserChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
