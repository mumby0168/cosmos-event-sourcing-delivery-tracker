using CleanArchitecture.Exceptions;
using DeliveryTracker.Domain.Abstractions.Entities;
using DeliveryTracker.Domain.Enums;
using DeliveryTracker.Domain.ValueObjects;

namespace DeliveryTracker.Domain.Entities;

public class Stop : IStop
{
    public Guid Id { get; }

    public Location Location { get; }

    public StopStatus Status { get; private set; }

    public DateTime? CompletedAt { get; private set; }

    public DateTime? AbandonedAt { get; private set; }

    public StopFailedDetails? FailedDetails { get; private set; }

    public Stop(Guid id, Location location)
    {
        Id = id;
        Location = location;
        Status = StopStatus.Outstanding;
    }

    public void Complete()
    {
        Status = StopStatus.Complete;
        CompletedAt = DateTime.UtcNow;
    }

    public void Failed(string reason)
    {
        Status = StopStatus.Failed;
        FailedDetails = new StopFailedDetails(DateTime.UtcNow, reason);
    }

    public void Abandoned()
    {
        Status = StopStatus.Abandoned;
        AbandonedAt = DateTime.UtcNow;
    }

    public void MarkFailed(string reason)
    {
        Status = StopStatus.Failed;
        FailedDetails = new StopFailedDetails(DateTime.UtcNow, reason);
    }

    public void MarkAbandoned(string reason)
    {
        AbandonedAt = DateTime.UtcNow;
        Status = StopStatus.Abandoned;
    }
}