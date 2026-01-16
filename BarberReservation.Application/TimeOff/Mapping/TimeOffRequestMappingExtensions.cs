using BarberReservation.Application.TimeOff.Queries.Admin.GetAllAdminTimeOffs;
using BarberReservation.Application.TimeOff.Queries.Hairdresser.GetAllSelfTimeOffs;
using BarberReservation.Shared.Models.TimeOff.Admin;
using BarberReservation.Shared.Models.TimeOff.Hairdresser;

namespace BarberReservation.Application.TimeOff.Mapping;

public static class TimeOffRequestMappingExtensions
{
    public static AdminHairdresserPagedRequest ToAdminHairdresserPagedRequest(this GetAllAdminTimeOffsQuery query)
    {
        return new AdminHairdresserPagedRequest
        {
            Desc = query.Desc,
            HairdresserId = query.HairdresserId,
            Page = query.Page,
            PageSize = query.PageSize,
            Search = query.Search,
            SortBy = query.SortBy
        };
    }

    public static HairdresserPagedRequest ToHairdresserPagedRequest(this GetAllSelfTimeOffsQuery query)
    {
        return new HairdresserPagedRequest
        {
            Desc = query.Desc,
            Page = query.Page,
            PageSize = query.PageSize,
            Search = query.Search,
            SortBy = query.SortBy
        };
    }
}
