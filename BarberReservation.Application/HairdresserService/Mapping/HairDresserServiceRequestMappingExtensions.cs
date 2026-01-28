using BarberReservation.Application.HairdresserService.Queries.Admin.GetAllAdminHairdressersServices;
using BarberReservation.Shared.Models.HairdresserService;

namespace BarberReservation.Application.HairdresserService.Mapping;

public static class HairdresserServiceRequestMappingExtensions
{
    public static CommonHairdresserServicePagedRequest ToHairdresserSelfServicePagedRequest(this Queries.Self.GetAllSelfHairdressersServices.GetAllSelfHairdressersServicesQuery query)
    {
        return new CommonHairdresserServicePagedRequest
        {
            Page = query.Page,
            PageSize = query.PageSize,
            Search = query.Search,
            SortBy = query.SortBy,
            Desc = query.Desc,
            IsActive = query.IsActive,
            ServiceId = query.ServiceId,
        };
    }

    public static CommonHairdresserServicePagedRequest ToHairdresserAdminServicePagedRequest(this GetAllAdminHairdressersServicesQuery query)
    {
        return new CommonHairdresserServicePagedRequest
        {
            Page = query.Page,
            PageSize = query.PageSize,
            Search = query.Search,
            HairdresserId = query.HairdresserId,
            ServiceId = query.ServiceId,
            IsActive = query.IsActive,
            SortBy = query.SortBy,
            Desc = query.Desc
        };
    }
}
