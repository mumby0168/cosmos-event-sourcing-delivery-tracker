using CleanArchitecture.Exceptions;
using DeliveryTracker.Application.Infrastructure;
using DeliveryTracker.Domain.Abstractions.Aggregates;
using DeliveryTracker.Domain.Aggregates;
using DeliveryTracker.Domain.Identifiers;
using DeliveryTracker.Infrastructure.Extensions;
using DeliveryTracker.Infrastructure.Items;
using Microsoft.Azure.CosmosEventSourcing.Stores;

namespace DeliveryTracker.Infrastructure.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly IEventStore<ScheduleEventItem> _eventStore;

    public ScheduleRepository(IEventStore<ScheduleEventItem> eventStore) => 
        _eventStore = eventStore;

    public ValueTask SaveAsync(ISchedule schedule)
    {
        var newEvents = schedule
            .NewEvents
            .Select(evt => new ScheduleEventItem(schedule.Driver.Code, schedule.Id, evt))
            .ToList();
        
        newEvents.Add(new ScheduleEventItem(
            schedule.Driver.Code, 
            schedule.Id, 
            schedule.AtomicEvent!));

        return _eventStore.PersistAsync(newEvents);
    }

    public async ValueTask<ISchedule> ReadAsync(ScheduleId scheduleId)
    {
        var events = await _eventStore
            .ReadAsync(scheduleId)
            .ToListAsync();

        if (!events.Any())
        {
            throw new ResourceNotFoundException<Schedule>(
                $"No schedule found with ID {scheduleId}");
        }

        return Schedule.Replay(events.Select(x => x.DomainEvent).ToList());
    }
}