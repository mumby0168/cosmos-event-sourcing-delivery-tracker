using DeliveryTracker.Domain.Abstractions.Events;
using DeliveryTracker.Domain.Identifiers;
using Microsoft.Azure.CosmosEventSourcing.Events;
using Microsoft.Azure.CosmosEventSourcing.Items;
using Newtonsoft.Json;

namespace DeliveryTracker.Infrastructure.Sources;

public class ScheduleEventItem : DefaultEventItem
{
    public ScheduleEventItem(
        string driverCode,
        ScheduleId scheduleId, 
        DomainEvent domainEvent) :
        base(domainEvent, scheduleId)
    {
        DriverCode = driverCode;
        if (domainEvent is IStopDomainEvent stop)
        {
            StopId = stop.StopId;
        }
    }
    
    public string DriverCode { get; set; } = null!;

    public Guid? StopId { get; set; }
    
    [JsonIgnore]
    public ScheduleId ScheduleId => PartitionKey;
    
    public ScheduleEventItem()
    {
        
    }
}