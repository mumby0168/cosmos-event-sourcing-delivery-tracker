using CleanArchitecture.Exceptions;
using DeliveryTracker.Domain.Entities;
using DeliveryTracker.Domain.Enums;
using DeliveryTracker.Domain.Events;
using DeliveryTracker.Domain.Events.Schedules;
using DeliveryTracker.Domain.Events.Stops;
using DeliveryTracker.Domain.ValueObjects;

namespace DeliveryTracker.Domain.Aggregates;

public partial class Schedule 
{
    public void AddStop(Location location) =>
        AddEvent(new StopScheduled(
            Guid.NewGuid(),
            location.HouseNumber,
            location.AddressLine,
            location.PostCode,
            DateTime.UtcNow));

    public void Start()
    {
        if (!Stops.Any())
        {
            throw new DomainException<Schedule>(
                "A schedule cannot be started without any stops");
        }
        
        AddEvent(new ScheduleStarted(
            Id,
            DateTime.UtcNow));
    }

    public void CompleteStop(Guid stopId)
    {
        if (Status is not ScheduleStatus.InProgress)
        {
            throw new DomainException<Schedule>(
                $"A stop cannot be completed when the schedule is at status {Status}. " +
                "The schedule must be in progress.");
        }

        var stop = GetStop(stopId);
        
        if (stop.Status is not StopStatus.Outstanding)
        {
            throw new DomainException<Stop>(
                $"The stop {Id} cannot be completed as it has a status of {Status}");
        }
        
        AddEvent(new StopCompleted(
            stopId,
            DateTime.UtcNow));
    }
    
    public void Abandon(string reason)
    {
        if (Status is not ScheduleStatus.InProgress)
        {
            throw new DomainException<Schedule>(
                $"The schedule {Id} cannot be abandoned while at status {Status}");
        }

        var at = DateTime.UtcNow;
        
        foreach (var outstandingStop in OutstandingStops)
        {
            AddEvent(new StopAbandoned(
                outstandingStop.Id,
                reason,
                at));
        }

        AddEvent(new ScheduleAbandoned(
            Id, 
            reason, 
            at));
    }

    public void FailStop(Guid stopId, string reason)
    {
        var stop = GetStop(stopId);

        if (stop.Status is not StopStatus.Outstanding)
        {
            throw new DomainException<Stop>(
                $"A stop cannot be mark as Failed when at status {Status}");
        }
        
        AddEvent(new StopFailed(
            stopId, 
            reason, 
            DateTime.UtcNow));
    }
}