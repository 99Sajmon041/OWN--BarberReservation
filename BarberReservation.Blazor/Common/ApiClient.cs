using BarberReservation.Blazor.Auth;
using BarberReservation.Blazor.Utils;
using System.Net.Http.Headers;

namespace BarberReservation.Blazor.Common;

public sealed class ApiClient(IHttpClientFactory factory, AuthState authState) : IApiClient
{
    private readonly HttpClient http = factory.CreateClient("Api");

    public async Task<T> GetAsync<T>(string url, CancellationToken ct)
    {
        using var msg = new HttpRequestMessage(HttpMethod.Get, url);
        AttachBearer(msg);

        var res = await http.SendAsync(msg, ct);
        if (!res.IsSuccessStatusCode)
            throw new ApiRequestException(await res.ReadProblemMessageAsync("Chyba API."), (int)res.StatusCode);

        var result = await res.Content.ReadFromJsonAsync<T>(ct)
            ?? throw new ApiRequestException("API returned no data.", (int)res.StatusCode);

        return result;
    }

    public async Task<TRes> PostAsyncWithResponse<TReq, TRes>(HttpMethod method, string url, TReq body, CancellationToken ct)
    {
        using var msg = new HttpRequestMessage(method, url);
        AttachBearer(msg);

        if (body is not null)
            msg.Content = JsonContent.Create(body);

        var res = await http.SendAsync(msg, ct);
        if (!res.IsSuccessStatusCode)
            throw new ApiRequestException(await res.ReadProblemMessageAsync("Chyba API."), (int)res.StatusCode);

        var result = await res.Content.ReadFromJsonAsync<TRes>(ct)
            ?? throw new ApiRequestException("API returned no data.", (int)res.StatusCode);

        return result;
    }

    public async Task SendAsync(HttpMethod method, string url, object? body, CancellationToken ct)
    {
        using var msg = new HttpRequestMessage(method, url);
        AttachBearer(msg);
        if (body is not null)
            msg.Content = JsonContent.Create(body);

        var res = await http.SendAsync(msg, ct);
        if (!res.IsSuccessStatusCode)
            throw new ApiRequestException(await res.ReadProblemMessageAsync("Chyba API."), (int)res.StatusCode);
    }

    private void AttachBearer(HttpRequestMessage msg)
    {
        var token = authState.Token;
        if (!string.IsNullOrWhiteSpace(token))
            msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
