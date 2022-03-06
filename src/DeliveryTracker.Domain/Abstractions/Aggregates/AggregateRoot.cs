using CleanArchitecture.Exceptions;
using Microsoft.Azure.CosmosEventSourcing;

namespace DeliveryTracker.Domain.Abstractions.Aggregates;

public abstract class AggregateRoot : IAggregateRoot
{
    private List<IPersistedEvent> _events = new();
    private readonly List<IPersistedEvent> _unSavedEvents = new();

    public IReadOnlyList<IPersistedEvent> UnSavedEvents =>
        _unSavedEvents;

    protected void AddEvent(IPersistedEvent persistedEvent)
    {
        Apply(persistedEvent);
        _unSavedEvents.Add(persistedEvent);
    }

    protected void Apply(List<IPersistedEvent> persistedEvents)
    {
        if (!persistedEvents.Any())
        {
            throw new DomainException<AggregateRoot>(
                $"At least one {nameof(IPersistedEvent)} must be provided");
        }

        var orderedEvents = persistedEvents
            .OrderBy(x => x.OccuredUtc)
            .ToList();

        orderedEvents.ForEach(Apply);
        _events = orderedEvents;
    }

    protected abstract void Apply(IPersistedEvent persistedEvent);
}