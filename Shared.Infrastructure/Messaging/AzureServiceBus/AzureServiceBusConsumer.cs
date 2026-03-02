namespace Shared.Infrastructure.Messaging.AzureServiceBus;

using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Infrastructure.Messaging.Abstractions;

public abstract class AzureServiceBusConsumer<TEvent> : IHostedService, IMessageConsumer where TEvent : class
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusProcessor _processor;
    private readonly ILogger<AzureServiceBusConsumer<TEvent>> _logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    protected AzureServiceBusConsumer(IOptions<AzureServiceBusOptions> options, ILogger<AzureServiceBusConsumer<TEvent>> logger)
    {
        _logger = logger;
        
        var asbOptions = options.Value;
        
        _client = new ServiceBusClient(asbOptions.ConnectionString);
        
        var processorOptions = new ServiceBusProcessorOptions
        {
            MaxConcurrentCalls = 10,
            AutoCompleteMessages = false // We handle completion manually (Retry-safe / Dead-letter compatible)
        };

        _processor = _client.CreateProcessor(asbOptions.TopicName, asbOptions.SubscriptionName, processorOptions);

        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        logger.LogInformation("AzureServiceBusConsumer initialized for Topic: {Topic}, Subscription: {Subscription}", 
            asbOptions.TopicName, asbOptions.SubscriptionName);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await StartConsumingAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await StopConsumingAsync(cancellationToken);
    }

    public async Task StartConsumingAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting ServiceBusProcessor for {EventName}...", typeof(TEvent).Name);
        await _processor.StartProcessingAsync(cancellationToken);
    }

    public async Task StopConsumingAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Stopping ServiceBusProcessor for {EventName}...", typeof(TEvent).Name);
        await _processor.StopProcessingAsync(cancellationToken);
        
        await _processor.DisposeAsync();
        await _client.DisposeAsync();
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        string body = args.Message.Body.ToString();
        var eventName = args.Message.Subject ?? "Unknown";
        
        // Basic routing filter to ensure we handle the correct event type
        if (eventName != typeof(TEvent).Name)
        {
            // If we're subscribed to a topic with multiple event types, simply ignore others
            await args.CompleteMessageAsync(args.Message);
            return;
        }

        _logger.LogDebug("Received Message: {MessageId} for Event: {EventName}", args.Message.MessageId, eventName);

        try
        {
            var @event = JsonSerializer.Deserialize<TEvent>(body, _jsonSerializerOptions);
            
            if (@event != null)
            {
                await ProcessMessageAsync(@event, args.CancellationToken);
                
                // Complete message if processed successfully
                await args.CompleteMessageAsync(args.Message);
            }
            else
            {
                _logger.LogWarning("Deserialized event was null. Moving to Dead Letter Queue.");
                await args.DeadLetterMessageAsync(args.Message, "DeserializationFailed", "Resulting object was null");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message {MessageId}. Moving to Dead Letter Queue.", args.Message.MessageId);
            // Move to DLQ on concrete processing failures for review
            await args.DeadLetterMessageAsync(args.Message, "ProcessingError", ex.Message);
        }
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "ServiceBus error occurred on Entity: {EntityPath}, ErrorSource: {ErrorSource}", 
            args.EntityPath, args.ErrorSource);
            
        return Task.CompletedTask;
    }

    // Abstract method that consumers in the bounded contexts must implement
    protected abstract Task ProcessMessageAsync(TEvent @event, CancellationToken cancellationToken);
}
