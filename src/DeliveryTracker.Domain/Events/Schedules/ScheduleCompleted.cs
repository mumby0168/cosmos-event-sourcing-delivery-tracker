using Microsoft.Azure.CosmosEventSourcing;

namespace DeliveryTracker.Domain.Events.Schedules;

public record ScheduleCompleted(
    string ScheduleId,
    bool IsPartiallyComplete,
    DateTime OccuredUtc) : IPersistedEvent
{
    public string EventName => nameof(ScheduleCompleted);
};