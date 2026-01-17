namespace BarberReservation.Application.TimeOff.Common;

public interface ITimeOffUpsert
{
    DateTime StartAt { get; }
    DateTime EndAt { get; }
    string Reason { get; }
}
