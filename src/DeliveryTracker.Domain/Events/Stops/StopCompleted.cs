using DeliveryTracker.Domain.Abstractions.Events;
using Microsoft.Azure.CosmosEventSourcing.Events;

namespace DeliveryTracker.Domain.Events.Stops;

public record StopCompleted(Guid StopId) : DomainEvent, IStopDomainEvent;