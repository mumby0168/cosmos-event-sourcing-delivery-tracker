using DeliveryTracker.Domain.Abstractions.Entities;
using DeliveryTracker.Domain.Enums;

namespace DeliveryTracker.Domain.Aggregates;

public partial class Schedule
{
    public IReadOnlyList<IStop> Stops => _stops;

    public IReadOnlyList<IStop> DeliveredStops => 
        _stops.Where(x => x.Status is StopStatus.Delivered).ToList();
    
    public IReadOnlyList<IStop> FailedStops =>
        _stops.Where(x => x.Status is StopStatus.Failed).ToList();
    
    public IReadOnlyList<IStop> AbandonedStops =>
        _stops.Where(x => x.Status is StopStatus.Abandoned).ToList();
    
    public IReadOnlyList<IStop> OutstandingStops =>
        _stops.Where(x => x.Status is StopStatus.Outstanding).ToList();

    public double Progress => CalculateProgress();

    private double CalculateProgress()
    {
        if (Status is ScheduleStatus.Scheduled)
        {
            return 0;
        }
        
        var deliverableStops = Stops.Count(x => x.Status is not StopStatus.Abandoned);
        var visitedStops = FailedStops.Count + DeliveredStops.Count;

        return visitedStops / deliverableStops * 100;
    }
}