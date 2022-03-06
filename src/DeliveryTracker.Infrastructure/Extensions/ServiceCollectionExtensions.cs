using DeliveryTracker.Application.Infrastructure;
using DeliveryTracker.Domain.Abstractions.Aggregates;
using DeliveryTracker.Infrastructure.Items;
using DeliveryTracker.Infrastructure.Projections;
using DeliveryTracker.Infrastructure.Repositories;
using DeliveryTracker.Infrastructure.Services;
using DeliveryTracker.Infrastructure.Sources;
using Microsoft.Azure.CosmosEventSourcing.Extensions;
using Microsoft.Azure.CosmosRepository.AspNetCore.Extensions;
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
                        c => 
                            c.WithServerlessThroughput())
                    .ConfigureProjectionStore<DriverSchedule>(
                        "projections",
                        containerOptionsBuilder: c => 
                            c.WithServerlessThroughput());
            });

            builder.AddEventSourceProjectionBuilder<ScheduleEventSource, DriverScheduleProjectionBuilder>();
            builder.AddAllPersistedEventsTypes(typeof(ISchedule).Assembly);
        });

        services.AddCosmosRepositoryChangeFeedHostedService();

        services.AddSingleton<IScheduleRepository, ScheduleRepository>();
        services.AddSingleton<IDriverScheduleReadService, DriverScheduleReadService>();

        return services;
    }
}