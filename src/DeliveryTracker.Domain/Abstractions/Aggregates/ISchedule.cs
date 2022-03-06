using DeliveryTracker.Domain.Abstractions.Entities;
using DeliveryTracker.Domain.Enums;
using DeliveryTracker.Domain.Identifiers;
using DeliveryTracker.Domain.ValueObjects;

namespace DeliveryTracker.Domain.Abstractions.Aggregates;

public interface ISchedule : IAggregateRoot
{
    ScheduleId Id { get; }
    
    Driver Driver { get; }
    
    ScheduleStatus Status { get; }
    
    IReadOnlyList<IStop> Stops { get; }
    
    IReadOnlyList<IStop> DeliveredStops { get; }
    
    IReadOnlyList<IStop> FailedStops { get; }
    
    IReadOnlyList<IStop> AbandonedStops { get; }
    
    IReadOnlyList<IStop> OutstandingStops { get; }

    double Progress { get; }

    void AddStop(Guid id, Location location);
}