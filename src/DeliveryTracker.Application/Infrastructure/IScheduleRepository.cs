using DeliveryTracker.Domain.Abstractions.Aggregates;
using DeliveryTracker.Domain.Identifiers;

namespace DeliveryTracker.Application.Infrastructure;

public interface IScheduleRepository
{
    ValueTask SaveAsync(ISchedule schedule);

    ValueTask<ISchedule> ReadAsync(ScheduleId scheduleId);
}