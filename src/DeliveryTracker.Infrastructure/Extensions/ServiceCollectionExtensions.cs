using DeliveryTracker.Application.Infrastructure;
using DeliveryTracker.Domain.Abstractions.Aggregates;
using DeliveryTracker.Infrastructure.Repositories;
using DeliveryTracker.Infrastructure.Sources;
using Microsoft.Azure.CosmosEventSourcing.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryTracker.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddCosmosEventSourcing(builder =>
        {
            builder.AddCosmosRepository(options =>
            {
                options.DatabaseId = "delivery-schedule-sample";

                options
                    .ContainerBuilder
                    .ConfigureEventSourceStore<ScheduleEventSource>(
                        "delivery-schedule-events", 
                        c => c.WithServerlessThroughput());
            });

            builder.AddAllPersistedEventsTypes(typeof(ISchedule).Assembly);
        });

        services.AddSingleton<IScheduleRepository, ScheduleRepository>();
        
        return services;
    }
}