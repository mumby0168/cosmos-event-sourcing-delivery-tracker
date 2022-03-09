using Microsoft.Azure.CosmosEventSourcing;
using Microsoft.Azure.CosmosEventSourcing.Events;

namespace DeliveryTracker.Domain.Events.Schedules;

public record ScheduleCompleted(
    string ScheduleId,
    bool IsPartiallyComplete) : DomainEvent;