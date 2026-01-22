using Microsoft.JSInterop;

namespace BarberReservation.Blazor.UI;

public static class ConfirmRequest
{
    public static async Task<bool> ConfirmAction(this IJSRuntime JS, string message)
    {
        return await JS.InvokeAsync<bool>("confirm", $"⚠️ {message}");
    }
}
