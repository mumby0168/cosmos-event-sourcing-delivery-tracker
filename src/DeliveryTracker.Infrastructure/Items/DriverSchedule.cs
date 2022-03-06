using DeliveryTracker.Domain.Identifiers;
using Microsoft.Azure.CosmosRepository;
using Newtonsoft.Json;

namespace DeliveryTracker.Infrastructure.Items;

public class DriverSchedule : FullItem
{
    public string PartitionKey { get; private set; }
    
    [JsonIgnore]
    public ScheduleId ScheduleId => Id;
    
    public string DriverCode { get; }
    
    public int TotalStops { get; set; }
    
    public int CompletedStops { get; set; }
    
    public bool IsInProgress { get; set; }

    public DriverSchedule(
        string id,
        string driverCode)
    {
        Id = id;
        PartitionKey = driverCode;
        DriverCode = driverCode;
    }

    protected override string GetPartitionKeyValue() =>
        PartitionKey;
}