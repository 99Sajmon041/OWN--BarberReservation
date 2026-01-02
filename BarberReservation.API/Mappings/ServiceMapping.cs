using BarberReservation.Application.Service.Commands.CreateService;
using BarberReservation.Application.Service.Commands.PartlyUpdateService;
using BarberReservation.Shared.Models.Service;

namespace BarberReservation.API.Mappings;

public static class ServiceMapping
{
    public static PartlyUpdateServiceCommand ToUpdateServiceCommand(this UpsertServiceRequest request, int id)
    {
        return new PartlyUpdateServiceCommand
        {
            Id = id,
            Name = request.Name,
            Description = request.Description
        };
    }

    public static CreateServiceCommand ToCreateServiceCommand(this UpsertServiceRequest request)
    {
        return new CreateServiceCommand
        {
            Name = request.Name,
            Description = request.Description
        };
    }
}
