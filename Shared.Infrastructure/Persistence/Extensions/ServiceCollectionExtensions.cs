namespace Shared.Infrastructure.Persistence.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Persistence.Abstractions;
using Shared.Infrastructure.Persistence.Mongo;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbOptions>(configuration.GetSection(MongoDbOptions.SectionName));

        services.AddScoped<IMongoDbContext, MongoDbContext>();
        services.AddScoped(typeof(IRepository<,>), typeof(MongoRepository<,>));

        return services;
    }
}
