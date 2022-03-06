namespace DeliveryTracker.Application.DTOs;

public record DriverScheduleDto(
    DateTime CreatedUtc,
    string ScheduleId,
    string DriverCode,
    int Stops,
    int CompletedStops,
    int FailedStops,
    int AbandonedStops,
    string Status);