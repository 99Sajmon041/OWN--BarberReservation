using AutoMapper;
using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.TimeOff.Commands.Hairdresser.UpdateSelfTimeOff;

public sealed class UpdateSelfTimeOffCommandHandler(
    ILogger<UpdateSelfTimeOffCommandHandler> logger,
    ICurrentAppUser currentAppUser,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateSelfTimeOffCommand>
{
    public async Task Handle(UpdateSelfTimeOffCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var hairdresser = currentAppUser.User;
        var freeFrom = request.StartAt;
        var freeTo = request.EndAt;

        var free = await unitOfWork.HairdresserTimeOffRepository.GetByIdAsync(request.Id, hairdresser.Id, ct);
        if(free is null)
        {
            logger.LogWarning("Hairdresser tries to find free with ID: {FreeId}, but does not exists. Hairdresser ID: {HairdresserId}", request.Id, hairdresser.Id);
            throw new NotFoundException("Volno nebylo nalezeno.");
        }

        if(free.StartAt.Date != freeFrom.Date)
        {
            logger.LogWarning("Hairdresser tries to change day, but it is not allowed. Hairdresser ID: {HairdresserId}, free ID: {FreeId}", hairdresser.Id, request.Id);
            throw new ConflictException("Nelze měnit den, pouze čas.");
        }

        if (freeFrom.Date != freeTo.Date)
        {
            logger.LogWarning("Hairdresser tries to set free, with cross over a day. Hairdresser ID: {HairdresserId}", hairdresser.Id);

            throw new ConflictException("Nelze aby volno přesahovalo do jiného dne.");
        }

        if (freeFrom >= freeTo)
        {
            logger.LogWarning("Hairdresser tries to set start of free after end of free. Hairdresser ID: {HairdresserId}, start-free: {FreeFrom}, end-free: {FreeTo}",
                hairdresser.Id,
                freeFrom.ToString("HH:mm"),
                freeTo.ToString("HH:mm"));

            throw new ConflictException("Nelze aby volno začínalo po jeho konci.");
        }

        var freeCanStart = DateTime.Now.AddHours(1);

        if (request.StartAt < freeCanStart)
        {
            logger.LogWarning("Hairdresser tries to set start of free febore actual time + 1 hour. Hairdresser: {HairdresserId}", hairdresser.Id);

            throw new ConflictException($"Volno může nejdříve začít v aktuální čas + 1 hodina. Nejdříve tedy: {freeCanStart:HH:mm}");
        }

        var workDay = await unitOfWork.HairdresserWorkingHoursRepository.GetEffectiveFromDayByTimeOffAsync(hairdresser.Id, DateOnly.FromDateTime(freeFrom), ct);
        if (workDay is null)
        {
            logger.LogInformation("Hairdresser tries to free, but no working-day was found. Hairdresser ID: {HairdresserId}, Day: {WorkingDay}",
                hairdresser.Id,
                freeTo.Date.ToString("yyyy.MM.dd"));

            throw new NotFoundException("Nelze nastavit volno, tento den pravděpodobně není pracovní / nebo plánujete moc dopředu.");
        }

        if (TimeOnly.FromDateTime(freeFrom) < workDay.WorkFrom || TimeOnly.FromDateTime(freeTo) > workDay.WorkTo)
        {
            logger.LogWarning(
                "Hairdresser tries to set free, budt free crosses working hours. Hairdresser ID: {HairdresserId}, start-free: {FreeFrom}, end-free: {FreeTo}, Work-day: {WorkDay}",
                hairdresser.Id,
                freeFrom.ToString("HH:mm"),
                freeTo.ToString("HH:mm"),
                freeFrom.ToString("yyyy.MM.dd"));

            throw new ConflictException("Nelze upravit volno, zasahuje do pracovní doby. Musí být uvnitř pracovní doby.");
        }

        var daysFree = await unitOfWork.HairdresserTimeOffRepository.GetAllByDayForHairdresserAsync(hairdresser.Id, DateOnly.FromDateTime(freeFrom), ct);
        foreach (var freeTime in daysFree)
        {
            if (freeTime.Id == request.Id)
                continue;

            if (freeTime.StartAt < freeTo && freeTime.EndAt > freeFrom)
            {
                logger.LogWarning(
                    "Hairdresser tries to update time off, but new interval overlaps with another time off. Hairdresser ID: {HairdresserId}," +
                    " Free ID: {FreeId}, NewFrom: {FreeFrom}, NewTo: {FreeTo}",
                        hairdresser.Id,
                        request.Id,
                        freeFrom.ToString("HH:mm"),
                        freeTo.ToString("HH:mm"));

                throw new ConflictException("Nelze upravit volno, překrývá se s již existujícím volnem.");
            }
        }

        bool overlap = await unitOfWork.ReservationRepository.ExistsOverlapForHairdresserAsync(hairdresser.Id, freeFrom, freeTo, ct);
        if (overlap)
        {
            logger.LogWarning(
                "Hairdresser tries to set free, but time is already filled by existing reservation. Hairdresser: {HairdresserId}, start-free: {FreeFrom}, end-free: {FreeTo}",
                hairdresser.Id,
                freeFrom.ToString("HH:mm"),
                freeTo.ToString("HH:mm"));

            throw new ConflictException("Nelze upravit volno, překrývá se s již vytvořenou rezervací.");
        }

        mapper.Map(request, free);

        await unitOfWork.SaveChangesAsync(ct);
    }
}
