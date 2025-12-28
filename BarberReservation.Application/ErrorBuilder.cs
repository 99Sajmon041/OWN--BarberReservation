using Microsoft.AspNetCore.Identity;

namespace BarberReservation.Application;

public static class ErrorBuilder
{
    public static string SetErrorMessage(IEnumerable<IdentityError> errors)
    {
        return string.Join(", ", errors.Select(e => e.Description));
    }
}
