using Microsoft.Azure.CosmosEventSourcing;
using Microsoft.Azure.CosmosEventSourcing.Events;

namespace DeliveryTracker.Domain.Abstractions.Aggregates;

public interface IAggregateRoot
{
    public IReadOnlyList<DomainEvent> UnSavedEvents { get; }
}