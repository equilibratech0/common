namespace Shared.Infrastructure.Persistence.Mongo;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Shared.Infrastructure.Persistence.Abstractions;

public class MongoDbContext : IMongoDbContext
{
    private static readonly object _initLock = new();
    private static bool _serializersRegistered;

    private readonly MongoClient _mongoClient;
    private readonly ILogger<MongoDbContext> _logger;

    public IMongoDatabase Database { get; }
    public IClientSessionHandle? Session { get; private set; }

    public MongoDbContext(IOptions<MongoDbOptions> options, ILogger<MongoDbContext> logger)
    {
        _logger = logger;
        
        RegisterSerializers();

        var mongoOptions = options.Value;
        
        _mongoClient = new MongoClient(mongoOptions.ConnectionString);
        Database = _mongoClient.GetDatabase(mongoOptions.DatabaseName);
        
        _logger.LogInformation("MongoDbContext initialized for database: {DatabaseName}", mongoOptions.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return Database.GetCollection<T>(name);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Starting MongoDB transaction...");
        Session = await _mongoClient.StartSessionAsync(cancellationToken: cancellationToken);
        Session.StartTransaction();
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Session is { IsInTransaction: true })
        {
            _logger.LogDebug("Committing MongoDB transaction...");
            await Session.CommitTransactionAsync(cancellationToken);
            Session.Dispose();
            Session = null;
        }
    }

    public async Task AbortTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Session is { IsInTransaction: true })
        {
            _logger.LogWarning("Aborting MongoDB transaction...");
            await Session.AbortTransactionAsync(cancellationToken);
            Session.Dispose();
            Session = null;
        }
    }

    private static void RegisterSerializers()
    {
        lock (_initLock)
        {
            if (_serializersRegistered) return;

            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            _serializersRegistered = true;
        }
    }
}
