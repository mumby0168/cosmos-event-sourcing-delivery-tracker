using DeliveryTracker.Domain.Abstractions.Entities;
using DeliveryTracker.Domain.Enums;
using DeliveryTracker.Domain.Identifiers;
using DeliveryTracker.Domain.ValueObjects;
using Microsoft.Azure.CosmosEventSourcing.Aggregates;

namespace DeliveryTracker.Domain.Abstractions.Aggregates;

public interface ISchedule : IAggregateRoot
{
    ScheduleId Id { get; }
    
    Driver Driver { get; }
    
    ScheduleStatus Status { get; }
    
    IReadOnlyList<IStop> Stops { get; }
    
    IReadOnlyList<IStop> CompletedStops { get; }
    
    IReadOnlyList<IStop> FailedStops { get; }
    
    IReadOnlyList<IStop> AbandonedStops { get; }
    
    IReadOnlyList<IStop> OutstandingStops { get; }

    double Progress { get; }

    void AddStop(Location location);

    void CompleteStop(Guid stopId);

    void FailStop(Guid stopId, string reason);

    void Start();
    
    void Abandon(string reason);
}