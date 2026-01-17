using AutoMapper;
using BarberReservation.Application.Exceptions;
using BarberReservation.Application.UserIdentity;
using BarberReservation.Domain.Entities;
using BarberReservation.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BarberReservation.Application.TimeOff.Commands.Hairdresser.CreateSelfTimeOff;

public sealed class CreateSelfTimeOffCommandHandler(
    ILogger<CreateSelfTimeOffCommandHandler> logger,
    ICurrentAppUser currentAppUser,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateSelfTimeOffCommand>
{
    public async Task<Unit> Handle(CreateSelfTimeOffCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var hairdresser = currentAppUser.User;
        var freeFrom = request.StartAt;
        var freeTo = request.EndAt;

        if(freeFrom.Date != freeTo.Date)
        {
            logger.LogWarning("Hairdresser tries to set free, with cross over a day. Hairdresser ID: {HairdresserId}",hairdresser.Id);

            throw new ConflictException("Nelze aby volno přesahovalo do jiného dne.");
        }

        if(freeFrom >= freeTo)
        {
            logger.LogWarning("Hairdresser tries to set start of free after end of free. Hairdresser ID: {HairdresserId}, start-free: {FreeFrom}, end-free: {FreeTo}",
                hairdresser.Id,
                freeFrom.ToString("HH:mm"),
                freeTo.ToString("HH:mm"));

            throw new ConflictException("Nelze aby volno začínalo po jeho konci.");
        }

        var freeCanStart = DateTime.UtcNow.AddHours(1);

        if (request.StartAt < freeCanStart)
        {
            logger.LogWarning("Hairdresser tries to set start of free febore actual time + 1 hour. Hairdresser: {HairdresserId}", hairdresser.Id);

            throw new ConflictException($"Volno může nejdříve začít v aktuální čas + 1 hodina. Nejdříve tedy: {freeCanStart:HH:mm}");
        }

        var workDay = await unitOfWork.HairdresserWorkingHoursRepository.GetEffectiveFromDayByTimeOffAsync(hairdresser.Id, DateOnly.FromDateTime(freeFrom), ct);
        if(workDay is null)
        {
            logger.LogInformation("Hairdresser tries to free, but no working-day was found. Hairdresser ID: {HairdresserId}, Day: {WorkingDay}",
                hairdresser.Id, 
                freeTo.Date.ToString("yyyy.MM.dd"));

            throw new NotFoundException("Nelze nastavit volno, tento den pravděpodobně není pracovní / nebo plánujete moc dopředu.");
        }

        if(TimeOnly.FromDateTime(freeFrom) < workDay.WorkFrom || TimeOnly.FromDateTime(freeTo) > workDay.WorkTo)
        {
            logger.LogWarning(
                "Hairdresser tries to set free, budt free crosses working hours. Hairdresser ID: {HairdresserId}, start-free: {FreeFrom}, end-free: {FreeTo}, Work-day: {WorkDay}",
                hairdresser.Id,
                freeFrom.ToString("HH:mm"),
                freeTo.ToString("HH:mm"),
                freeFrom.ToString("yyyy.MM.dd"));

            throw new ConflictException("Nelze vytvořit volno, zasahuje do pracovní doby. Musí být uvnitř ní.");
        }

        var daysFree = await unitOfWork.HairdresserTimeOffRepository.GetAllByDayForHairdresserAsync(hairdresser.Id, freeFrom, ct);
        foreach (var freeTime in daysFree)
        {
            if (freeTime.StartAt < freeTo && freeTime.EndAt > freeFrom)
            {
                logger.LogWarning(
                    "Hairdresser tries to create time off, but new interval overlaps with another time off. Hairdresser ID: {HairdresserId}," +
                    " NewFrom: {FreeFrom}, NewTo: {FreeTo}",
                        hairdresser.Id,
                        freeFrom.ToString("HH:mm"),
                        freeTo.ToString("HH:mm"));

                throw new ConflictException("Nelze vytvořit volno, překrývá se s již existujícím volnem.");
            }
        }

        bool overlap = await unitOfWork.ReservationRepository.ExistsOverlapForHairdresserAsync(hairdresser.Id, freeFrom, freeTo, ct);
        if(overlap)
        {
            logger.LogWarning(
                "Hairdresser tries to set free, but time is already filled by existing reservation. Hairdresser: {HairdresserId}, start-free: {FreeFrom}, end-free: {FreeTo}",
                hairdresser.Id,
                freeFrom.ToString("HH:mm"),
                freeTo.ToString("HH:mm"));

            throw new ConflictException("Nelze vytvořit volno, překrývá se s již vytvořenou rezervací.");
        }

        var timeOff = mapper.Map<HairdresserTimeOff>(request);

        unitOfWork.HairdresserTimeOffRepository.Add(timeOff);

        await unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
