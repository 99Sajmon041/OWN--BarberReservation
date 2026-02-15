using BarberReservation.Application.TimeOff.Commands.Hairdresser.CreateSelfTimeOff;
using BarberReservation.Application.TimeOff.Commands.Hairdresser.UpdateSelfTimeOff;
using BarberReservation.Application.TimeOff.Queries.Admin.GetAllAdminTimeOffs;
using BarberReservation.Application.TimeOff.Queries.Hairdresser.GetAllSelfTimeOffs;
using BarberReservation.Shared.Models.TimeOff;

namespace BarberReservation.Application.TimeOff.Mapping;

public static class TimeOffRequestMappingExtensions
{
    public static HairdresserTimeOffPagedRequest ToAdminHairdresserPagedRequest(this GetAllAdminTimeOffsQuery query)
    {
        return new HairdresserTimeOffPagedRequest
        {
            Desc = query.Desc,
            HairdresserId = query.HairdresserId,
            Page = query.Page,
            PageSize = query.PageSize,
            Search = query.Search,
            SortBy = query.SortBy,
            Year = query.Year,
            Month = query.Month
        };
    }

    public static HairdresserTimeOffPagedRequest ToHairdresserPagedRequest(this GetAllSelfTimeOffsQuery query)
    {
        return new HairdresserTimeOffPagedRequest
        {
            Desc = query.Desc,
            Page = query.Page,
            PageSize = query.PageSize,
            Search = query.Search,
            SortBy = query.SortBy,
            Year = query.Year,
            Month = query.Month
        };
    }

    public static CreateSelfTimeOffCommand ToCreateSelfTimeOffCommand(this UpsertTimeOffRequest request)
    {
        return new CreateSelfTimeOffCommand
        {
            StartAt = request.StartAt,
            EndAt = request.EndAt,
            Reason = request.Reason.Trim(),
        };
    }

    public static UpdateSelfTimeOffCommand ToUpdateSelfTimeOffCommand(this UpsertTimeOffRequest request, int id)
    {
        return new UpdateSelfTimeOffCommand(id)
        {
            StartAt = request.StartAt,
            EndAt = request.EndAt,
            Reason = request.Reason.Trim(),
        };
    }
}
