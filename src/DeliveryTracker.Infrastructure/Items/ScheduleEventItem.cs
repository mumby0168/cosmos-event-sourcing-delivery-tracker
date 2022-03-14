using DeliveryTracker.Domain.Abstractions.Events;
using DeliveryTracker.Domain.Identifiers;
using Microsoft.Azure.CosmosEventSourcing.Events;
using Microsoft.Azure.CosmosEventSourcing.Items;
using Newtonsoft.Json;

namespace DeliveryTracker.Infrastructure.Items;

public class ScheduleEventItem : EventItem
{
    public ScheduleEventItem(
        string driverCode,
        ScheduleId scheduleId, 
        DomainEvent domainEvent)
    {
        PartitionKey = scheduleId.ToString();
        DomainEvent = domainEvent;
        DriverCode = driverCode;
        if (domainEvent is IStopDomainEvent stop)
        {
            StopId = stop.StopId;
        }
    }
    
    public string DriverCode { get; set; }

    public Guid? StopId { get; set; }
    
    [JsonIgnore]
    public ScheduleId ScheduleId => PartitionKey;
}