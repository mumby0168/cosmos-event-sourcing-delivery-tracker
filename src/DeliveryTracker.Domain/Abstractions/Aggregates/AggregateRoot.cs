using CleanArchitecture.Exceptions;
using Microsoft.Azure.CosmosEventSourcing.Events;

namespace DeliveryTracker.Domain.Abstractions.Aggregates;

public abstract class AggregateRoot : IAggregateRoot
{
    private List<DomainEvent> _events = new();
    private readonly List<DomainEvent> _unSavedEvents = new();

    public IReadOnlyList<DomainEvent> UnSavedEvents =>
        _unSavedEvents;

    protected void AddEvent(DomainEvent persistedEvent)
    {
        Apply(persistedEvent);
        _unSavedEvents.Add(persistedEvent);
    }

    protected void Apply(List<DomainEvent> persistedEvents)
    {
        if (!persistedEvents.Any())
        {
            throw new DomainException<AggregateRoot>(
                $"At least one {nameof(DomainEvent)} must be provided");
        }

        var orderedEvents = persistedEvents
            .OrderBy(x => x.OccuredUtc)
            .ToList();

        orderedEvents.ForEach(Apply);
        _events = orderedEvents;
    }

    protected abstract void Apply(DomainEvent persistedEvent);
}