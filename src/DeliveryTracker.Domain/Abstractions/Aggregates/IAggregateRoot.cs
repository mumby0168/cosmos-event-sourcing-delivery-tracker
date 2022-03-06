using Microsoft.Azure.CosmosEventSourcing;

namespace DeliveryTracker.Domain.Abstractions.Aggregates;

public interface IAggregateRoot
{
    public IReadOnlyList<IPersistedEvent> UnSavedEvents { get; }
}