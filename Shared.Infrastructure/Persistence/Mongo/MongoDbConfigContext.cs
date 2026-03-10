namespace Shared.Infrastructure.Persistence.Mongo;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Shared.Infrastructure.Persistence.Abstractions;

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
        _serializersRegistered = true;
    }
}
