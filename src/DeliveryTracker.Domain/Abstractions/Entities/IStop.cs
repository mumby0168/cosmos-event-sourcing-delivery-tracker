using DeliveryTracker.Domain.Enums;
using DeliveryTracker.Domain.ValueObjects;

namespace DeliveryTracker.Domain.Abstractions.Entities;

public interface IStop
{
    Guid Id { get; }
    
    StopStatus Status { get; }
    
    Location Location { get; }
    
    DateTime? DeliveredAt { get; }
    
    DateTime? AbandonedAt { get; }
    
    StopFailedDetails? FailedDetails { get; }

    void Delivered(DateTime at);

    void Failed(DateTime at, string reason);

    void Abandoned(DateTime at);
}