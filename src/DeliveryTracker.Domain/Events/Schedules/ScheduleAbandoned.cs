using Microsoft.Azure.CosmosEventSourcing;
using Microsoft.Azure.CosmosEventSourcing.Events;

namespace DeliveryTracker.Domain.Events.Schedules;

public record ScheduleAbandoned(
    string ScheduleId,
    string Reason) : DomainEvent;