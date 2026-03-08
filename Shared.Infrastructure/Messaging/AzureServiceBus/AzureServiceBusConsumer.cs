namespace Shared.Infrastructure.Messaging.AzureServiceBus;

using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Infrastructure.Messaging.Abstractions;

public abstract class AzureServiceBusConsumer<TEvent> : IHostedService, IMessageConsumer where TEvent : class
{
    private const int MaxRetries = 1;

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
            AutoCompleteMessages = false,
            PrefetchCount = 20
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
        
        if (eventName != typeof(TEvent).Name)
        {
            await args.CompleteMessageAsync(args.Message);
            return;
        }

        _logger.LogDebug("Received Message: {MessageId} for Event: {EventName}", args.Message.MessageId, eventName);

        try
        {
            var @event = JsonSerializer.Deserialize<TEvent>(body, _jsonSerializerOptions);
            
            if (@event is null)
            {
                _logger.LogWarning("Deserialized event was null. Moving to Dead Letter Queue.");
                await args.DeadLetterMessageAsync(args.Message, "DeserializationFailed", "Resulting object was null");
                return;
            }

            await ProcessWithRetryAsync(@event, args);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize message {MessageId}. Moving to Dead Letter Queue.", args.Message.MessageId);
            await args.DeadLetterMessageAsync(args.Message, "DeserializationFailed", ex.Message);
        }
    }

    private async Task ProcessWithRetryAsync(TEvent @event, ProcessMessageEventArgs args)
    {
        int attempt = 0;

        while (true)
        {
            try
            {
                attempt++;
                await ProcessMessageAsync(@event, args.CancellationToken);
                await args.CompleteMessageAsync(args.Message);
                return;
            }
            catch (Exception ex) when (attempt <= MaxRetries)
            {
                _logger.LogWarning(ex,
                    "Error processing message {MessageId} on attempt {Attempt}/{MaxAttempts}. Retrying...",
                    args.Message.MessageId, attempt, MaxRetries + 1);

                await Task.Delay(500 * attempt, args.CancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error processing message {MessageId} after {Attempts} attempts. Moving to Dead Letter Queue.",
                    args.Message.MessageId, attempt);

                await args.DeadLetterMessageAsync(args.Message, "ProcessingError", ex.Message);
                return;
            }
        }
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "ServiceBus error occurred on Entity: {EntityPath}, ErrorSource: {ErrorSource}", 
            args.EntityPath, args.ErrorSource);
            
        return Task.CompletedTask;
    }

    protected abstract Task ProcessMessageAsync(TEvent @event, CancellationToken cancellationToken);
}
