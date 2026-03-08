namespace Shared.Infrastructure.Persistence.Abstractions;

using MongoDB.Driver;

public interface IMongoDbConfigContext
{
    IMongoCollection<T> GetCollection<T>(string name);
}
