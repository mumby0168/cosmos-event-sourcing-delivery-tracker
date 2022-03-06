using DeliveryTracker.Domain.Abstractions.Aggregates;
using DeliveryTracker.Domain.Abstractions.Entities;
using DeliveryTracker.Domain.Enums;
using DeliveryTracker.Domain.Events;
using DeliveryTracker.Domain.Events.Schedules;
using DeliveryTracker.Domain.Identifiers;
using DeliveryTracker.Domain.ValueObjects;

namespace DeliveryTracker.Domain.Aggregates;

public partial class Schedule : AggregateRoot, ISchedule
{
    private readonly List<IStop> _stops = new();
    public ScheduleId Id { get; private set; } = null!;

    public Driver Driver { get; private set; } = null!;

    public ScheduleStatus Status { get; private set; }

    public Schedule(Driver driver)
    {
        Driver = driver;
        Id = ScheduleId.NewScheduleId(driver.Code);
        Status = ScheduleStatus.Scheduled;
        
        AddEvent(new ScheduleCreated(
            Id,
            driver.Code,
            driver.FirstName,
            driver.SecondName,
            DateTime.UtcNow));
    }

    public Schedule()
    {
        
    }
}