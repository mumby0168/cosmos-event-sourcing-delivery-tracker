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
        if (Status is not StopStatus.Outstanding)
        {
            throw new DomainException<Stop>(
                $"The stop {Id} cannot be completed as it has a status of {Status}");
        }
        
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
}