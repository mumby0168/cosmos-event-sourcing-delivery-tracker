using Microsoft.Azure.CosmosEventSourcing;

namespace DeliveryTracker.Domain.Aggregates;

public partial class Schedule 
{
    protected override void Apply(IPersistedEvent persistedEvent)
    {
        throw new NotImplementedException();
    }
}