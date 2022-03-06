using DeliveryTracker.Domain.Abstractions.Events;

namespace DeliveryTracker.Domain.Events.Stops;

public record StopScheduled(
    Guid StopId,
    int HouseNumber, 
    string AddressLine, 
    string PostCode,
    DateTime OccuredUtc) : IStopPersistedEvent
{
    public string EventName => nameof(StopScheduled);
}