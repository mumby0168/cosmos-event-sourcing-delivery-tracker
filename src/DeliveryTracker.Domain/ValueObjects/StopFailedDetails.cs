namespace DeliveryTracker.Domain.ValueObjects;

public record StopFailedDetails(
    DateTime FailedAt,
    string Reason);