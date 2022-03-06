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

    protected void Apply(StopScheduled stopScheduled)
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
}