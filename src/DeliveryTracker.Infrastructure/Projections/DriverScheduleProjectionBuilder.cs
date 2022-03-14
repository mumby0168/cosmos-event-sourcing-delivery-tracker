using DeliveryTracker.Domain.Events.Schedules;
using DeliveryTracker.Domain.Events.Stops;
using DeliveryTracker.Infrastructure.Items;
using Microsoft.Azure.CosmosEventSourcing.Projections;
using Microsoft.Azure.CosmosRepository;

namespace DeliveryTracker.Infrastructure.Projections;

public class DriverScheduleProjectionBuilder : IEventItemProjectionBuilder<ScheduleEventItem>
{
    private readonly IRepository<DriverSchedule> _repository;

    public DriverScheduleProjectionBuilder(IRepository<DriverSchedule> repository) =>
        _repository = repository;


    public async ValueTask ProjectAsync(
        ScheduleEventItem item,
        CancellationToken cancellationToken)
    {
        var task = item.DomainEvent switch
        {
            ScheduleCreated created => HandleScheduleCreated(created),
            StopScheduled => HandleStopScheduled(await GetSchedule(item)),
            ScheduleStarted => HandleScheduleStarted(await GetSchedule(item)),
            StopCompleted => HandleStopCompleted(await GetSchedule(item)),
            StopAbandoned => HandleStopAbandoned(await GetSchedule(item)),
            StopFailed => HandleStopFailed(await GetSchedule(item)),
            ScheduleCompleted completed => HandleScheduleCompleted(completed, await GetSchedule(item)),
            ScheduleAbandoned => HandleScheduleAbandoned(await GetSchedule(item)),
            _ => ValueTask.CompletedTask
        };

        await task;
    }

    private async ValueTask HandleScheduleCompleted(
        ScheduleCompleted completed,
        DriverSchedule schedule)
    {
        schedule.IsInProgress = false;
        if (completed.IsPartiallyComplete)
        {
            schedule.IsPartiallyComplete = true;
        }
        else
        {
            schedule.IsComplete = true;
        }

        await _repository.UpdateAsync(schedule);
    }

    private async ValueTask HandleScheduleAbandoned(
        DriverSchedule schedule)
    {
        schedule.IsInProgress = false;
        schedule.IsAbandoned = true;
        await _repository.UpdateAsync(schedule);
    }

    private async ValueTask HandleStopCompleted(
        DriverSchedule schedule)
    {
        schedule.CompletedStops++;
        await _repository.UpdateAsync(schedule);
    }

    private async ValueTask HandleStopAbandoned(
        DriverSchedule schedule)
    {
        schedule.AbandonedStops++;
        await _repository.UpdateAsync(schedule);
    }

    private async ValueTask HandleStopFailed(
        DriverSchedule schedule)
    {
        schedule.FailedStops++;
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

    private ValueTask<DriverSchedule> GetSchedule(ScheduleEventItem s) =>
        _repository.GetAsync(s.ScheduleId, s.DriverCode);
}