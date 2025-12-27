namespace BarberReservation.Shared.Models.Authorization;

public sealed class LoginResponse
{
    public string? Token { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public bool MustChangePassword { get; set; }
}
