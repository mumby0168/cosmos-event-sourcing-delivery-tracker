using DeliveryTracker.Domain.Events;
using DeliveryTracker.Domain.Identifiers;
using DeliveryTracker.Infrastructure.Items;
using DeliveryTracker.Infrastructure.Sources;
using Microsoft.Azure.CosmosEventSourcing.Projections;
using Microsoft.Azure.CosmosRepository;

namespace DeliveryTracker.Infrastructure.Projections;

public class DriverScheduleProjectionBuilder : IEventSourceProjectionBuilder<ScheduleEventSource>
{
    private readonly IRepository<DriverSchedule> _repository;

    public DriverScheduleProjectionBuilder(IRepository<DriverSchedule> repository) =>
        _repository = repository;


    public async ValueTask ProjectAsync(
        ScheduleEventSource source,
        CancellationToken cancellationToken)
    {
        var task = source.EventPayload switch
        {
            ScheduleCreated created => HandleScheduleCreated(created),
            StopScheduled => HandleStopScheduled(await GetSchedule(source)),
            ScheduleStarted => HandleScheduleStarted(await GetSchedule(source)),
            StopCompleted => HandleStopCompleted(await GetSchedule(source)),
            _ => ValueTask.CompletedTask
        };

        await task;
    }

    private async ValueTask HandleStopCompleted(
        DriverSchedule schedule)
    {
        schedule.CompletedStops++;
        await _repository.UpdateAsync(schedule);
    }

    private async ValueTask HandleStopScheduled(
        DriverSchedule schedule)
    {
        schedule.TotalStops++;
        await _repository.UpdateAsync(schedule);
    }

    private async ValueTask HandleScheduleStarted(
        DriverSchedule schedule)
    {
        schedule.IsInProgress = true;
        await _repository.UpdateAsync(schedule);
    }

    private async ValueTask HandleScheduleCreated(
        ScheduleCreated created) =>
        await _repository.CreateAsync(new DriverSchedule(
            created.Id,
            created.DriverCode));

    private ValueTask<DriverSchedule> GetSchedule(ScheduleEventSource s) =>
        _repository.GetAsync(s.ScheduleId, s.DriverCode);
}