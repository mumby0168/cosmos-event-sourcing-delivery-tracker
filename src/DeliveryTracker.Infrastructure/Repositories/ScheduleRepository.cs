using CleanArchitecture.Exceptions;
using DeliveryTracker.Application.Infrastructure;
using DeliveryTracker.Domain.Abstractions.Aggregates;
using DeliveryTracker.Domain.Aggregates;
using DeliveryTracker.Domain.Identifiers;
using DeliveryTracker.Infrastructure.Extensions;
using DeliveryTracker.Infrastructure.Sources;
using Microsoft.Azure.CosmosEventSourcing;

namespace DeliveryTracker.Infrastructure.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly IEventStore<ScheduleEventSource> _eventStore;

    public ScheduleRepository(IEventStore<ScheduleEventSource> eventStore) => 
        _eventStore = eventStore;

    public ValueTask SaveAsync(ISchedule schedule)
    {
        var newEvents = schedule
            .UnSavedEvents
            .Select(x => new ScheduleEventSource(schedule.Id, x));

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

        return Schedule.Replay(events.Select(x => x.EventPayload).ToList());
    }
}