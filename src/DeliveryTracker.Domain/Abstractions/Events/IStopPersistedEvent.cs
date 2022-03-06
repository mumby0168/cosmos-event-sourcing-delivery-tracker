using Microsoft.Azure.CosmosEventSourcing;

namespace DeliveryTracker.Domain.Abstractions.Events;

public interface IStopPersistedEvent : IPersistedEvent
{
    public Guid StopId { get; }
}