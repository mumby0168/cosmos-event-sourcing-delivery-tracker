using Microsoft.Azure.CosmosEventSourcing;
using Microsoft.Azure.CosmosEventSourcing.Events;

namespace DeliveryTracker.Domain.Abstractions.Events;

public interface IStopDomainEvent : IDomainEvent
{
    public Guid StopId { get; }
}