using DeliveryTracker.Domain.Abstractions.Events;
using Microsoft.Azure.CosmosEventSourcing;

namespace DeliveryTracker.Domain.Events;

public record StopScheduled(
    Guid Id,
    int HouseNumber, 
    string AddressLine, 
    string PostCode,
    DateTime OccuredUtc) : IStopPersistedEvent
{
    public string EventName => nameof(StopScheduled);
}