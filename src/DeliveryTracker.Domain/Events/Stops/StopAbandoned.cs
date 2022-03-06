using DeliveryTracker.Domain.Abstractions.Events;

namespace DeliveryTracker.Domain.Events.Stops;

public record StopAbandoned(
    Guid StopId,
    string Reason,
    DateTime OccuredUtc) : IStopPersistedEvent
{
    public string EventName => nameof(StopAbandoned);
}