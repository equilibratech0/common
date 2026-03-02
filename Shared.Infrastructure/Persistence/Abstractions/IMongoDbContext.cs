namespace Shared.Infrastructure.Persistence.Abstractions;

using MongoDB.Driver;

public interface IMongoDbContext
{
    IMongoDatabase Database { get; }
    IMongoCollection<T> GetCollection<T>(string name);
    
    // For transactions and unit of work
    IClientSessionHandle? Session { get; }
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task AbortTransactionAsync(CancellationToken cancellationToken = default);
}
