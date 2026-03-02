namespace Shared.Infrastructure.Messaging.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Messaging.Abstractions;
using Shared.Infrastructure.Messaging.AzureServiceBus;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzureServiceBusInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureServiceBusOptions>(configuration.GetSection(AzureServiceBusOptions.SectionName));

        // Register Publisher as Scoped or Singleton. Usually Singleton is preferred for ASB clients/senders.
        services.AddSingleton<IMessagePublisher, AzureServiceBusPublisher>();

        return services;
    }
}
