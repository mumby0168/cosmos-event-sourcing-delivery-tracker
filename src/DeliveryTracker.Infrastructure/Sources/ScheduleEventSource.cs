using DeliveryTracker.Domain.Abstractions.Events;
using DeliveryTracker.Domain.Identifiers;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Azure.CosmosEventSourcing;
using Newtonsoft.Json;

namespace DeliveryTracker.Infrastructure.Sources;

public class ScheduleEventSource : EventSource
{

    public ScheduleEventSource(
        ScheduleId scheduleId, 
        IPersistedEvent persistedEvent) :
        base(persistedEvent, scheduleId)
    {
        if (persistedEvent is IStopPersistedEvent stop)
        {
            StopId = stop.Id;
        }
    }
    
    public Guid? StopId { get; private set; }
    
    [JsonIgnore]
    public ScheduleId ScheduleId => PartitionKey;
    
    public ScheduleEventSource()
    {
        
    }
}