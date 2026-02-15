namespace BarberReservation.Application.TimeOff.Common;

public interface ITimeOffDailyListFilter
{
    DateTime WeekStartDate { get; }
}
