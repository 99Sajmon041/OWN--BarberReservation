namespace BarberReservation.Application.TimeOff.Common;

public interface ITimeOffListFilter
{
    int? Year { get; }
    int? Month { get; }
}
