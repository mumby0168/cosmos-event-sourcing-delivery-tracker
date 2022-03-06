using Convey.CQRS.Queries;
using DeliveryTracker.Application.DTOs;
using DeliveryTracker.Application.Infrastructure;

namespace DeliveryTracker.Application.Queries;

public static class FetchDriverSchedules
{
    public record Query(string DriverCode) : IQuery<IEnumerable<DriverScheduleDto>>;
    
    public class Handler : IQueryHandler<Query, IEnumerable<DriverScheduleDto>>
    {
        private readonly IDriverScheduleReadService _readService;

        public Handler(IDriverScheduleReadService readService) => 
            _readService = readService;

        public async Task<IEnumerable<DriverScheduleDto>>
            HandleAsync(Query query, CancellationToken cancellationToken) =>
            await _readService.GetAsync(query.DriverCode);
    }
}