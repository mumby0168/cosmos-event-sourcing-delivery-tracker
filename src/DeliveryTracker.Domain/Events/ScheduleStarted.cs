using Microsoft.Azure.CosmosEventSourcing;

namespace DeliveryTracker.Domain.Events;

public record ScheduleStarted(
    string ScheduleId,
    DateTime OccuredUtc) : IPersistedEvent
{
    public string EventName => nameof(ScheduleStarted);
};