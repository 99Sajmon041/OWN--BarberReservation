namespace BarberReservation.Blazor.Common;

public interface IApiClient
{
    Task<T> GetAsync<T>(string url, CancellationToken ct);
    Task SendAsync(HttpMethod method, string url, object? body, CancellationToken ct);
    Task<TRes> PostAsyncWithResponse<TReq, TRes>(HttpMethod method, string url, TReq body, CancellationToken ct);
}