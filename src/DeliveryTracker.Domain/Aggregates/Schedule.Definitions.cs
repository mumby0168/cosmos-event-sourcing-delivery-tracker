using DeliveryTracker.Domain.Abstractions.Aggregates;
using DeliveryTracker.Domain.Abstractions.Entities;
using DeliveryTracker.Domain.Enums;
using DeliveryTracker.Domain.Identifiers;
using DeliveryTracker.Domain.ValueObjects;

namespace DeliveryTracker.Domain.Aggregates;

public partial class Schedule : AggregateRoot, ISchedule
{
    private readonly List<IStop> _stops = new();
    public ScheduleId Id { get; }
    
    public Driver Driver { get; private set; }
    
    public ScheduleStatus Status { get; }

    public Schedule(Driver driver)
    {
        Driver = driver;
        Id = ScheduleId.NewScheduleId(driver.Code);
        Status = ScheduleStatus.Scheduled;
    }
}