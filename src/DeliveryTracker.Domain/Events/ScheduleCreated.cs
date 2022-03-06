using DeliveryTracker.Domain.Identifiers;
using Microsoft.Azure.CosmosEventSourcing;

namespace DeliveryTracker.Domain.Events;

public record ScheduleCreated(
    string Id,
    string DriverCode,
    string DriverFirstName,
    string DriverSecondName,
    DateTime OccuredUtc) : IPersistedEvent
{
    public string EventName => nameof(ScheduleCreated);
}