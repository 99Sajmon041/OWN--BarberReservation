using BarberReservation.Application.Common.Validation.PagingValidation;
using BarberReservation.Application.Common.Validation.SearchValidation;
using FluentValidation;

namespace BarberReservation.Application.Service.Queries.GetAllServices;

public sealed class GetAllServicesQueryValidator : AbstractValidator<GetAllServicesQuery> 
{
    public GetAllServicesQueryValidator()
    {
        Include(new PagingValidator<GetAllServicesQuery>());
        Include(new SearchValidator<GetAllServicesQuery>());
    }
}
