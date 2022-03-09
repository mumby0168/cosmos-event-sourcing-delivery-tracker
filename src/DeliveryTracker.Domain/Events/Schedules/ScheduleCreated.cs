using Microsoft.Azure.CosmosEventSourcing;
using Microsoft.Azure.CosmosEventSourcing.Events;

namespace DeliveryTracker.Domain.Events.Schedules;

public record ScheduleCreated(
    string Id,
    string DriverCode,
    string DriverFirstName,
    string DriverSecondName) : DomainEvent;