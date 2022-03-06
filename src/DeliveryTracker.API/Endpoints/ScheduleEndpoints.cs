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
        builder.MapPost(Endpoint(), DispatchCommand<CreateSchedule.Command>)
            .Produces(200)
            .Produces<ErrorResponse>(400)
            .WithTags(Tag);
        
        builder.MapPost(Endpoint("/stops"), DispatchCommand<ScheduleStop.Command>)
            .Produces(200)
            .Produces<ErrorResponse>(400)
            .WithTags(Tag);
        
        builder.MapPut(Endpoint("/start"), DispatchCommand<StartSchedule.Command>)
            .Produces(200)
            .Produces<ErrorResponse>(400)
            .WithTags(Tag);

        builder.MapPut(Endpoint("/stops/complete"), DispatchCommand<CompleteStop.Command>)
            .Produces(200)
            .Produces<ErrorResponse>(400)
            .WithTags(Tag);

        return builder;
    }

    private static Task DispatchCommand<T>(
        T command,
        ICommandDispatcher dispatcher,
        CancellationToken cancellationToken) where T : class, ICommand =>
        dispatcher.SendAsync(command, cancellationToken);
}