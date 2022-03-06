using Microsoft.Azure.CosmosEventSourcing;

namespace DeliveryTracker.Domain.Events.Schedules;

public record ScheduleCreated(
    string Id,
    string DriverCode,
    string DriverFirstName,
    string DriverSecondName,
    DateTime OccuredUtc) : IPersistedEvent
{
    public string EventName => nameof(ScheduleCreated);
}