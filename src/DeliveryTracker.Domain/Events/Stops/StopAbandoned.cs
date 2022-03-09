using DeliveryTracker.Domain.Abstractions.Events;
using Microsoft.Azure.CosmosEventSourcing.Events;

namespace DeliveryTracker.Domain.Events.Stops;

public record StopAbandoned(
    Guid StopId,
    string Reason) : DomainEvent, IStopDomainEvent;