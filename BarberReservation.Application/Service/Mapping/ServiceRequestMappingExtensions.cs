using BarberReservation.Application.Service.Queries.GetAllServices;
using BarberReservation.Shared.Models.Service;

namespace BarberReservation.Application.Service.Mapping;

public static class ServiceRequestMappingExtensions
{
    public static ServicePageRequest ToServicePageRequest(this GetAllServicesQuery query)
    {
        return new ServicePageRequest
        {
            IsActive = query.IsActive,
            Search = query.Search,
            SortBy = query.SortBy,
            Desc = query.Desc,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }
}
