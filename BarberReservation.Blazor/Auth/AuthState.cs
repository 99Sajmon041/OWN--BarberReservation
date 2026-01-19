using BarberReservation.Shared.Enums;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using System.Text.Json;

namespace BarberReservation.Blazor.Auth;

public sealed class AuthState
{
    private const string StorageKey = "auth.session";
    private readonly ProtectedSessionStorage _session;
    private readonly ProtectedLocalStorage _local;
    private string? _token;
    private ClaimsPrincipal? _principalCache;

    public string? Token =>  !string.IsNullOrWhiteSpace(_token) && ExpiresAt > DateTime.UtcNow ? _token : null;
    public DateTime ExpiresAt { get; private set; }
    public bool MustChangePassword { get; private set; }
    public bool IsAuthenticated => Token is not null;

    public event Action? OnChange;

    public AuthState(ProtectedSessionStorage session, ProtectedLocalStorage local)
    {
        _session = session;
        _local = local;
    }

    public ClaimsPrincipal Principal
    {
        get
        {
            if (!IsAuthenticated)
                return new ClaimsPrincipal(new ClaimsIdentity());

            if (_principalCache is not null)
                return _principalCache;

            var claims = JwtClaimsParser.ParseClaimsFromJwt(Token!);
            _principalCache = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            return _principalCache;
        }
    }

    public string FullName => Principal.Identity?.Name ?? "Uživatel";

    public IReadOnlyList<string> Roles
    {
        get
        {
            var roles = Principal.FindAll(ClaimTypes.Role).Select(x => x.Value).ToList();
            roles = ExpandIfJsonArray(roles);
            if (roles.Count > 0)
                return roles;

            var schemaRoles = Principal
                .FindAll("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                .Select(x => x.Value)
                .ToList();
            schemaRoles = ExpandIfJsonArray(schemaRoles);
            if (schemaRoles.Count > 0)
                return schemaRoles;

            var raw = Principal.FindFirst("role")?.Value ?? Principal.FindFirst("roles")?.Value;
            if (!string.IsNullOrWhiteSpace(raw) && raw.TrimStart().StartsWith("["))
            {
                try
                {
                    var parsed = JsonSerializer.Deserialize<List<string>>(raw);
                    if (parsed is { Count: > 0 })
                        return parsed;
                }
                catch 
                { 
                }
            }

            return Array.Empty<string>();
        }
    }

    public string Role
    {
        get
        {
            if (Roles.Count == 0) 
                return "Role";

            return Roles.Contains(nameof(UserRoles.Admin)) ? nameof(UserRoles.Admin) : Roles[0];
        }
    }

    public async Task LoadAsync()
    {
        var local = await _local.GetAsync<AuthSession>(StorageKey);
        var session = await _session.GetAsync<AuthSession>(StorageKey);

        var data = local.Success ? local.Value : (session.Success ? session.Value : null);
        if (data is null)
            return;

        _token = data.Token;
        ExpiresAt = data.ExpiresAt;
        MustChangePassword = data.MustChangePassword;

        _principalCache = null;
        OnChange?.Invoke();
    }

    public async Task SetSessionAsync(string token, DateTime expiresAt, bool mustChangePassword, bool rememberMe)
    {
        _token = token;
        ExpiresAt = expiresAt;
        MustChangePassword = mustChangePassword;

        var data = new AuthSession(token, expiresAt, mustChangePassword);

        if (rememberMe)
        {
            await _local.SetAsync(StorageKey, data);
            await _session.DeleteAsync(StorageKey);
        }
        else
        {
            await _session.SetAsync(StorageKey, data);
            await _local.DeleteAsync(StorageKey);
        }

        _principalCache = null;
        OnChange?.Invoke();
    }

    public async Task MarkPasswordChangedAsync()
    {
        MustChangePassword = false;

        var rememberMe = (await _local.GetAsync<AuthSession>(StorageKey)).Success;
        if (_token is not null)
        {
            await SetSessionAsync(_token, ExpiresAt, MustChangePassword, rememberMe);
        }

        OnChange?.Invoke();
    }

    public async Task ClearAsync()
    {
        _token = null;
        ExpiresAt = default;
        MustChangePassword = false;

        await _session.DeleteAsync(StorageKey);
        await _local.DeleteAsync(StorageKey);

        _principalCache = null;
        OnChange?.Invoke();
    }

    private static List<string> ExpandIfJsonArray(List<string> roles)
    {
        if (roles.Count != 1) return roles;

        var raw = roles[0];
        if (string.IsNullOrWhiteSpace(raw) || !raw.TrimStart().StartsWith("[")) 
            return roles;

        try
        {
            var parsed = JsonSerializer.Deserialize<List<string>>(raw);
            return parsed is { Count: > 0 } ? parsed : roles;
        }
        catch
        {
            return roles;
        }
    }

    private sealed record AuthSession(string Token, DateTime ExpiresAt, bool MustChangePassword);
}
