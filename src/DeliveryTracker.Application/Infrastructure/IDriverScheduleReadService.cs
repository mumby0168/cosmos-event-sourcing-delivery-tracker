using DeliveryTracker.Application.DTOs;

namespace DeliveryTracker.Application.Infrastructure;

public interface IDriverScheduleReadService
{
    ValueTask<IEnumerable<DriverScheduleDto>> GetAsync(string driverCode);
}