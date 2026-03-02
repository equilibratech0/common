namespace Shared.Infrastructure.Messaging.Abstractions;

using Shared.Domain.Events;

public interface IMessagePublisher
{
    Task PublishIntegrationEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class;
}
