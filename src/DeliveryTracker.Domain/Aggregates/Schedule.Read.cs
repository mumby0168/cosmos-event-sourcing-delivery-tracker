using CleanArchitecture.Exceptions;
using DeliveryTracker.Domain.Abstractions.Entities;
using DeliveryTracker.Domain.Entities;
using DeliveryTracker.Domain.Enums;

namespace DeliveryTracker.Domain.Aggregates;

public partial class Schedule
{
    public IReadOnlyList<IStop> Stops => _stops;

    public IReadOnlyList<IStop> CompletedStops => 
        _stops.Where(x => x.Status is StopStatus.Complete).ToList();
    
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
        var visitedStops = FailedStops.Count + CompletedStops.Count;

        return visitedStops / deliverableStops * 100;
    }

    private IStop GetStop(Guid stopId)
    {
        var stop = Stops.FirstOrDefault(x => x.Id == stopId);

        if (stop is null)
        {
            throw new ResourceNotFoundException<Stop>(
                $"A stop with the ID {stopId} was not found on schedule {Id}");
        }

        return stop;
    }
}