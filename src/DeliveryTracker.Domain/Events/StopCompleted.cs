using DeliveryTracker.Domain.Abstractions.Events;

namespace DeliveryTracker.Domain.Events;

public record StopCompleted(
    Guid StopId,
    DateTime OccuredUtc
    ) : IStopPersistedEvent
{
    public string EventName => nameof(StopCompleted);
}