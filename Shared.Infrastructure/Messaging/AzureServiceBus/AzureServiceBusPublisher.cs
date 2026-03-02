namespace Shared.Infrastructure.Messaging.AzureServiceBus;

using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Infrastructure.Messaging.Abstractions;

public class AzureServiceBusPublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusSender _sender;
    private readonly ILogger<AzureServiceBusPublisher> _logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public AzureServiceBusPublisher(IOptions<AzureServiceBusOptions> options, ILogger<AzureServiceBusPublisher> logger)
    {
        _logger = logger;
        
        var asbOptions = options.Value;
        
        _client = new ServiceBusClient(asbOptions.ConnectionString);
        _sender = _client.CreateSender(asbOptions.TopicName);
        
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
        
        _logger.LogInformation("AzureServiceBusPublisher initialized for Topic: {TopicName}", asbOptions.TopicName);
    }

    public async Task PublishIntegrationEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class
    {
        await PublishMessageAsync(@event, "IntegrationEvent", cancellationToken);
    }

    private async Task PublishMessageAsync<TEvent>(TEvent @event, string eventTypeLabel, CancellationToken cancellationToken)
    {
        var eventName = @event.GetType().Name;
        
        _logger.LogDebug("Publishing {EventTypeLabel}: {EventName}", eventTypeLabel, eventName);

        var messageBody = JsonSerializer.Serialize(@event, _jsonSerializerOptions);
        
        var serviceBusMessage = new ServiceBusMessage(messageBody)
        {
            MessageId = Guid.NewGuid().ToString(),
            Subject = eventName,
            ContentType = "application/json",
            ApplicationProperties =
            {
                { "EventType", eventName },
                { "EventCategory", eventTypeLabel },
                { "EventVersion", "1.0" } // Support for basic versioning
            }
        };

        try
        {
            await _sender.SendMessageAsync(serviceBusMessage, cancellationToken);
            _logger.LogInformation("Successfully published {EventName} to Azure Service Bus.", eventName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish {EventName} to Azure Service Bus.", eventName);
            throw; // Fail fast so caller knows
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_sender != null)
        {
            await _sender.DisposeAsync();
        }
        
        if (_client != null)
        {
            await _client.DisposeAsync();
        }
    }
}
