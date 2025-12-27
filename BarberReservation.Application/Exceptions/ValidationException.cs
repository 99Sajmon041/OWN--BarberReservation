namespace BarberReservation.Application.Exceptions;

public sealed class ValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(string message, IDictionary<string, string[]> errors) : base(message)
    {
        Errors = new Dictionary<string, string[]>(errors);
    }

    public ValidationException(IDictionary<string, string[]> errors) : base("Validace selhala.")
    {
        Errors = new Dictionary<string, string[]>(errors);
    }
}
