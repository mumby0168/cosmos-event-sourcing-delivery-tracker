using Convey.CQRS.Queries;
using DeliveryTracker.Application.DTOs;
using DeliveryTracker.Application.Queries;

namespace DeliveryTracker.API.Endpoints;

public static class DriverEndpoints
{
    private const string Tag = "Driver";

    public static IEndpointRouteBuilder MapDriverAPIEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet(
            "/api/v1/drivers/{driverCode}/schedules",
            (string driverCode, IQueryDispatcher dispatcher) =>
                dispatcher.QueryAsync(new FetchDriverSchedules.Query(driverCode)))
            .WithTags(Tag);
        
        return builder;
    }
}