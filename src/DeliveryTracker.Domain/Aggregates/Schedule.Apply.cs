using CleanArchitecture.Exceptions;
using DeliveryTracker.Domain.Abstractions.Aggregates;
using DeliveryTracker.Domain.Entities;
using DeliveryTracker.Domain.Enums;
using DeliveryTracker.Domain.Events;
using DeliveryTracker.Domain.ValueObjects;
using Microsoft.Azure.CosmosEventSourcing;

namespace DeliveryTracker.Domain.Aggregates;

public partial class Schedule
{
    public static ISchedule Replay(List<IPersistedEvent> events)
    {
        var s = new Schedule();
        s.Apply(events);
        return s;
    }

    protected override void Apply(IPersistedEvent persistedEvent)
    {
        switch (persistedEvent)
        {
            case ScheduleCreated created:
                Apply(created);
                break;
            case StopScheduled stopScheduled:
                Apply(stopScheduled);
                break;
            case StopCompleted stopCompleted:
                Apply(stopCompleted);
                break;
            case ScheduleStarted started:
                Apply(started);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(persistedEvent),
                    $"There is no {nameof(Apply)} method configured for {typeof(IPersistedEvent)} of type {persistedEvent.GetType().Name}");
        }
    }

    private void Apply(ScheduleCreated scheduleCreated)
    {
        var (
            id,
            driverCode,
            driverFirstName,
            driverSecondName, _) = scheduleCreated;

        Id = id;
        Status = ScheduleStatus.Scheduled;

        Driver = new Driver(
            driverCode,
            driverFirstName,
            driverSecondName);
    }

    private void Apply(StopScheduled stopScheduled)
    {
        var (
            id,
            houseNumber,
            addressLine,
            postCode, _) = stopScheduled;

        var location = new Location(
            houseNumber,
            addressLine,
            postCode);

        _stops.Add(new Stop(
            id,
            location));
    }

    private void Apply(StopCompleted stopCompleted)
    {
        if (Status is not ScheduleStatus.InProgress)
        {
            throw new DomainException<Schedule>(
                $"A stop cannot be completed when the schedule is at status {Status}. " +
                "The schedule must be in progress.");
        }


        var (stopId, at) = stopCompleted;

        var stop = Stops.FirstOrDefault(x => x.Id == stopId);

        if (stop is null)
        {
            throw new ResourceNotFoundException<Stop>(
                $"A stop with the ID {stopId} was not found on schedule {Id}");
        }

        stop.Complete(at);
    }

    private void Apply(ScheduleStarted _) => 
        Status = ScheduleStatus.InProgress;
}