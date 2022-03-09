using DeliveryTracker.Domain.Abstractions.Aggregates;
using DeliveryTracker.Domain.Entities;
using DeliveryTracker.Domain.Enums;
using DeliveryTracker.Domain.Events.Schedules;
using DeliveryTracker.Domain.Events.Stops;
using DeliveryTracker.Domain.ValueObjects;
using Microsoft.Azure.CosmosEventSourcing.Events;

namespace DeliveryTracker.Domain.Aggregates;

public partial class Schedule
{
    public static ISchedule Replay(List<DomainEvent> events)
    {
        var s = new Schedule();
        s.Apply(events);
        return s;
    }

    protected override void Apply(DomainEvent persistedEvent)
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
            case ScheduleAbandoned scheduleAbandoned:
                Apply(scheduleAbandoned);
                break;
            case StopFailed stopFailed:
                Apply(stopFailed);
                break;
            case StopAbandoned stopAbandoned:
                Apply(stopAbandoned);
                break;
            case ScheduleCompleted completed:
                Apply(completed);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(persistedEvent),
                    $"There is no {nameof(Apply)} method configured " +
                    $"for {typeof(DomainEvent)} " +
                    $"of type {persistedEvent.GetType().Name}");
        }
    }

    private void Apply(ScheduleCreated scheduleCreated)
    {
        var (
            id,
            driverCode,
            driverFirstName,
            driverSecondName) = scheduleCreated;

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
            postCode) = stopScheduled;

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
        var stop = Stops.First(x => x.Id == stopCompleted.StopId);
        stop.Complete();
        TryMarkComplete();
    }

    private void Apply(ScheduleStarted _) =>
        Status = ScheduleStatus.InProgress;

    private void Apply(StopFailed stopFailed)
    {
        var (stopId, reason) = stopFailed;
        var stop = GetStop(stopId);
        stop.MarkFailed(reason);
        TryMarkComplete();
    }

    private void Apply(StopAbandoned stopAbandoned)
    {
        var (stopId, reason) = stopAbandoned;
        var stop = GetStop(stopId);
        stop.MarkAbandoned(reason);
    }

    private void Apply(ScheduleAbandoned _) =>
        Status = ScheduleStatus.Abandoned;

    private void Apply(ScheduleCompleted completed) =>
        Status = completed.IsPartiallyComplete
            ? ScheduleStatus.PartiallyComplete
            : ScheduleStatus.Complete;

    private void TryMarkComplete()
    {
        if (OutstandingStops.Any())
        {
            return;
        }

        if (Status is ScheduleStatus.Abandoned)
        {
            return;
        }

        var partiallyComplete = !Stops.All(x =>
            x.Status is StopStatus.Complete);

        AddEvent(new ScheduleCompleted(
            Id,
            partiallyComplete));
    }
}