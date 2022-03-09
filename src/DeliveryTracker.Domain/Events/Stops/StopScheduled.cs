using DeliveryTracker.Domain.Abstractions.Events;
using Microsoft.Azure.CosmosEventSourcing.Events;

namespace DeliveryTracker.Domain.Events.Stops;

public record StopScheduled(
    Guid StopId,
    int HouseNumber,
    string AddressLine,
    string PostCode) : DomainEvent, IStopDomainEvent;