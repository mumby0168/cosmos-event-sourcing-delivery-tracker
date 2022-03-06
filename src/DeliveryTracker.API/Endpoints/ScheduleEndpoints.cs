using CleanArchitecture.Exceptions.AspNetCore;
using Convey.CQRS.Commands;
using DeliveryTracker.Application.Commands;

namespace DeliveryTracker.API.Endpoints;

public static class ScheduleEndpoints
{
    private static string Endpoint(string? suffix = null) => $"/api/v1/schedules{suffix}";
    private const string Tag = "Schedule";

    public static IEndpointRouteBuilder MapScheduleAPIEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost(Endpoint(), CreateSchedule)
            .Produces(200)
            .Produces<ErrorResponse>(400)
            .WithTags(Tag);
        
        builder.MapPost(Endpoint("/stops"), ScheduleStop)
            .Produces(200)
            .Produces<ErrorResponse>(400)
            .WithTags(Tag);

        return builder;
    }

    private static Task CreateSchedule(
        CreateSchedule.Command command,
        ICommandDispatcher dispatcher,
        CancellationToken cancellationToken) =>
        dispatcher.SendAsync(command, cancellationToken);

    private static Task ScheduleStop(
        ScheduleStop.Command command,
        ICommandDispatcher dispatcher,
        CancellationToken cancellationToken) =>
        dispatcher.SendAsync(command, cancellationToken);
}