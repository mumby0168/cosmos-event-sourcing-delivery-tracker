using DeliveryTracker.Domain.Abstractions.Events;
using DeliveryTracker.Domain.Identifiers;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Azure.CosmosEventSourcing;
using Newtonsoft.Json;

namespace DeliveryTracker.Infrastructure.Sources;

public class ScheduleEventSource : EventSource
{
    public ScheduleEventSource(
        string driverCode,
        ScheduleId scheduleId, 
        IPersistedEvent persistedEvent) :
        base(persistedEvent, scheduleId)
    {
        DriverCode = driverCode;
        if (persistedEvent is IStopPersistedEvent stop)
        {
            StopId = stop.StopId;
        }
    }
    
    public string DriverCode { get; set; } = null!;

    public Guid? StopId { get; set; }
    
    [JsonIgnore]
    public ScheduleId ScheduleId => PartitionKey;
    
    public ScheduleEventSource()
    {
        
    }
}