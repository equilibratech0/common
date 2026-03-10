namespace Shared.Infrastructure.Persistence.Mongo;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Shared.Infrastructure.Persistence.Abstractions;
using Domain = Shared.Domain;

public class MongoDbConfigContext : IMongoDbConfigContext
{
    private static bool _serializersRegistered;

    private readonly IMongoDatabase _database;

    public MongoDbConfigContext(IOptions<MongoDbConfigOptions> options, ILogger<MongoDbConfigContext> logger)
    {
        RegisterSerializers();

        var mongoOptions = options.Value;
        var client = new MongoClient(mongoOptions.ConnectionString);
        _database = client.GetDatabase(mongoOptions.DatabaseName);

        logger.LogInformation("MongoDbConfigContext initialized for database: {DatabaseName}", mongoOptions.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name) => _database.GetCollection<T>(name);

    private static void RegisterSerializers()
    {
        if (_serializersRegistered) return;

        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        RegisterClassMap<Domain.Entities.User>();
        RegisterClassMap<Domain.Entities.Account>();
        RegisterClassMap<Domain.Entities.Company>();
        RegisterClassMap<Domain.Entities.Subscription>();
        RegisterClassMap<Domain.Entities.UserAccount>();
        RegisterClassMap<Domain.Entities.TransactionIngestionModel>();
        RegisterClassMap<Domain.Entities.Amount>();

        _serializersRegistered = true;
    }

    private static void RegisterClassMap<T>()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
        {
            BsonClassMap.RegisterClassMap<T>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}
