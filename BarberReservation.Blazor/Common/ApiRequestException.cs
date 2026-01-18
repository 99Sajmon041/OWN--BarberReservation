namespace BarberReservation.Blazor.Common;

public sealed class ApiRequestException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}
