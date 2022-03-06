using DeliveryTracker.Domain.Abstractions.Entities;
using DeliveryTracker.Domain.Enums;
using DeliveryTracker.Domain.ValueObjects;

namespace DeliveryTracker.Domain.Entities;

public class Stop : IStop
{
    public Guid Id { get; }
    
    public Location Location { get; }
    
    public StopStatus Status { get; private set; }
    
    public DateTime? DeliveredAt { get; private set; }

    public DateTime? AbandonedAt { get; private set; }
    
    public StopFailedDetails? FailedDetails { get; private set; }

    public Stop(Guid id, Location location)
    {
        Id = id;
        Location = location;
        Status = StopStatus.Outstanding;
    }

    public void Delivered(DateTime at)
    {
        Status = StopStatus.Delivered;
        DeliveredAt = at;
    }

    public void Failed(DateTime at, string reason)
    {
        Status = StopStatus.Failed;
        FailedDetails = new StopFailedDetails(at, reason);
    }

    public void Abandoned(DateTime at)
    {
        Status = StopStatus.Abandoned;
        AbandonedAt = at;
    }
}