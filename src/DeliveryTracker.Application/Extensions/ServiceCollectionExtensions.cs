using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryTracker.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddConvey()
            .AddQueryHandlers()
            .AddCommandHandlers()
            .AddInMemoryCommandDispatcher()
            .AddInMemoryQueryDispatcher();
        
        return services;
    }
}