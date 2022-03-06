using DeliveryTracker.Application.DTOs;
using DeliveryTracker.Application.Infrastructure;
using DeliveryTracker.Infrastructure.Items;
using Microsoft.Azure.CosmosRepository;

namespace DeliveryTracker.Infrastructure.Services;

public class DriverScheduleReadService : IDriverScheduleReadService
{
    private readonly IRepository<DriverSchedule> _repository;

    public DriverScheduleReadService(IRepository<DriverSchedule> repository) =>
        _repository = repository;

    public async ValueTask<IEnumerable<DriverScheduleDto>> GetAsync(string driverCode)
    {
        var results = await _repository.GetAsync(
            x => x.PartitionKey == driverCode);
        
        return results.Select(x =>
            new DriverScheduleDto(
                x.CreatedTimeUtc!.Value,
                x.ScheduleId,
                x.DriverCode,
                x.TotalStops,
                x.CompletedStops,
                x.FailedStops,
                x.AbandonedStops,
                x switch
                {
                    {IsInProgress: true} => "In Progress 🚚",
                    {IsPartiallyComplete: true} => "Partially Completed 🌓",
                    {IsAbandoned: true} => "Abandoned 🛑",
                    {IsComplete: true} => "Complete ✅",
                    _ => throw new ArgumentOutOfRangeException(nameof(x))
                }));
    }
}