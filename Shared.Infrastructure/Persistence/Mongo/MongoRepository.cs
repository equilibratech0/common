namespace Shared.Infrastructure.Persistence.Mongo;

using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Shared.Domain.Entities;
using Shared.Infrastructure.Persistence.Abstractions;

public class MongoRepository<TAggregateRoot, TId> : IRepository<TAggregateRoot, TId>
    where TAggregateRoot : AggregateRoot<TId>
{
    protected readonly IMongoDbContext DbContext;
    protected readonly IMongoCollection<TAggregateRoot> Collection;
    private readonly ILogger<MongoRepository<TAggregateRoot, TId>> _logger;

    public MongoRepository(IMongoDbContext dbContext, ILogger<MongoRepository<TAggregateRoot, TId>> logger)
    {
        DbContext = dbContext;
        _logger = logger;
        
        // Dynamically resolve collection name from entity type (e.g. "PayIn" -> "pay_ins" or just "PayIns")
        string collectionName = typeof(TAggregateRoot).Name + "s";
        Collection = dbContext.GetCollection<TAggregateRoot>(collectionName);
    }

    public virtual async Task<TAggregateRoot?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching {EntityType} with Id: {Id}", typeof(TAggregateRoot).Name, id);
        
        var filter = Builders<TAggregateRoot>.Filter.Eq(x => x.Id, id);
        return await Collection.Find(DbContext.Session, filter).SingleOrDefaultAsync(cancellationToken);
    }

    public virtual async Task AddAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Adding new {EntityType} with Id: {Id}", typeof(TAggregateRoot).Name, aggregate.Id);
        
        if (DbContext.Session != null)
        {
            await Collection.InsertOneAsync(DbContext.Session, aggregate, cancellationToken: cancellationToken);
        }
        else
        {
            await Collection.InsertOneAsync(aggregate, cancellationToken: cancellationToken);
        }
    }

    public virtual async Task UpdateAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Updating {EntityType} with Id: {Id}", typeof(TAggregateRoot).Name, aggregate.Id);

        var filter = Builders<TAggregateRoot>.Filter.Eq(x => x.Id, aggregate.Id);
        
        if (DbContext.Session != null)
        {
            await Collection.ReplaceOneAsync(DbContext.Session, filter, aggregate, cancellationToken: cancellationToken);
        }
        else
        {
            await Collection.ReplaceOneAsync(filter, aggregate, cancellationToken: cancellationToken);
        }
    }

    public virtual async Task DeleteAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Deleting {EntityType} with Id: {Id}", typeof(TAggregateRoot).Name, aggregate.Id);

        var filter = Builders<TAggregateRoot>.Filter.Eq(x => x.Id, aggregate.Id);
        
        if (DbContext.Session != null)
        {
            await Collection.DeleteOneAsync(DbContext.Session, filter, cancellationToken: cancellationToken);
        }
        else
        {
            await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        }
    }
}
