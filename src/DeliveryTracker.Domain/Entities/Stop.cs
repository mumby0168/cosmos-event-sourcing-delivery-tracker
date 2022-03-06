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

    public void Complete(DateTime at)
    {
        Status = StopStatus.Complete;
        CompletedAt = at;
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

    public void MarkFailed(string reason, DateTime at)
    {
        Status = StopStatus.Failed;
        FailedDetails = new StopFailedDetails(at, reason);
    }

    public void MarkAbandoned(string reason, DateTime at)
    {
        AbandonedAt = at;
        Status = StopStatus.Abandoned;
    }
}