namespace Shared.Infrastructure.Messaging.Abstractions;

public interface IMessageConsumer
{
    Task StartConsumingAsync(CancellationToken cancellationToken = default);
    Task StopConsumingAsync(CancellationToken cancellationToken = default);
}
