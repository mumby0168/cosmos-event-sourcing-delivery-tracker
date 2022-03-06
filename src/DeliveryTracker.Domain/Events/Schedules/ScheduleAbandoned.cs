using Microsoft.Azure.CosmosEventSourcing;

namespace DeliveryTracker.Domain.Events.Schedules;

public record ScheduleAbandoned(
    string ScheduleId,
    string Reason,
    DateTime OccuredUtc) : IPersistedEvent
{
    public string EventName => nameof(ScheduleAbandoned);
}