namespace BarberReservation.Blazor.Auth;

public sealed class AuthState
{
    private string? _token;
    public string? Token => !string.IsNullOrWhiteSpace(_token) && ExpiresAt > DateTime.UtcNow ? _token : null;
    public DateTime ExpiresAt { get; private set; }
    public bool MustChangePassword { get; private set; }
    public bool IsAuthenticated => Token is not null;
    public event Action? OnChange;

    public void SetSession(string token, DateTime expiresAt, bool mustChangePassword)
    {
        _token = token;
        ExpiresAt = expiresAt;
        MustChangePassword = mustChangePassword;
        OnChange?.Invoke();
    }

    public void MarkPasswordChanged()
    {
        MustChangePassword = false;
        OnChange?.Invoke();
    }

    public void Clear()
    {
        _token = null;
        ExpiresAt = default;
        MustChangePassword = false;
        OnChange?.Invoke();
    }
}
