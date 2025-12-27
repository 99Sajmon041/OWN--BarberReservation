namespace BarberReservation.Application.Exceptions;

public sealed class DomainException(string message) : Exception(message)
{
}
