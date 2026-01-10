using BarberReservation.Application.HairdresserService.Queries.Admin.GetAllAdminHairdressersServices;
using BarberReservation.Shared.Models.HairdresserService.Admin;
using BarberReservation.Shared.Models.HairdresserService.Self;

namespace BarberReservation.Application.HairdresserService.Mapping;

public static class HairdresserServiceRequestMappingExtensions
{
    public static HairdresserSelfServicePagedRequest ToHairDresserSelfServicePagedRequest(this Queries.Self.GetAllSelfHairdressersServices.GetAllSelfHairdressersServicesQuery query)
    {
        return new HairdresserSelfServicePagedRequest
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

    public static HairdresserAdminServicePagedRequest ToHairdresserAdminServicePagedRequest(this GetAllAdminHairdressersServicesQuery query)
    {
        return new HairdresserAdminServicePagedRequest
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
