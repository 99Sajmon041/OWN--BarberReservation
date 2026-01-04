namespace BarberReservation.Application.Common.Validation.PagingValidation;

public interface IHasPaging
{
    int Page { get; }
    int PageSize { get; }
}
