namespace BarberReservation.Blazor.Common;

public sealed class ApiClient(HttpClient httpClient)
{
    public HttpClient Http { get; } = httpClient;
}
