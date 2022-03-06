using DeliveryTracker.Domain.Events;
using DeliveryTracker.Domain.ValueObjects;

namespace DeliveryTracker.Domain.Aggregates;

public partial class Schedule 
{
    public void AddStop(Location location) =>
        AddEvent(new StopScheduled(
            Guid.NewGuid(),
            location.HouseNumber,
            location.AddressLine,
            location.PostCode,
            DateTime.UtcNow));
}