namespace Shared.Infrastructure.Http;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

public static class ServiceCollectionExtensions
{
    public static IHttpClientBuilder AddResilientHttpClient<TClient, TImplementation>(this IServiceCollection services, string clientName)
        where TClient : class
        where TImplementation : class, TClient
    {
        // Add a typed HTTP client with standard resilience policies (Timeout, Retry, Circuit Breaker)
        var builder = services.AddHttpClient<TClient, TImplementation>(clientName);
        
        builder.AddStandardResilienceHandler();
        
        return builder;
    }
}
