namespace Shared.Infrastructure.Persistence.Abstractions;

using Shared.Domain.Entities;

public interface IRepository<TAggregateRoot, TId> where TAggregateRoot : AggregateRoot<TId>
{
    Task<TAggregateRoot?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task AddAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default);
    Task UpdateAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default);
    Task DeleteAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default);
}
