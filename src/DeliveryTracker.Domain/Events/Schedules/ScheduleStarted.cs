
using Microsoft.Azure.CosmosEventSourcing.Events;

namespace DeliveryTracker.Domain.Events.Schedules;

public record ScheduleStarted(string ScheduleId) : DomainEvent;