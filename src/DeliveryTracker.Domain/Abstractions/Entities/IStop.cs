using DeliveryTracker.Domain.Enums;
using DeliveryTracker.Domain.ValueObjects;

namespace DeliveryTracker.Domain.Abstractions.Entities;

public interface IStop
{
    Guid Id { get; }
    
    StopStatus Status { get; }
    
    Location Location { get; }
    
    DateTime? CompletedAt { get; }
    
    DateTime? AbandonedAt { get; }
    
    StopFailedDetails? FailedDetails { get; }

    void Complete();

    void Failed(string reason);

    void Abandoned();
    
    void MarkFailed(string reason);
    
    void MarkAbandoned(string reason);
}