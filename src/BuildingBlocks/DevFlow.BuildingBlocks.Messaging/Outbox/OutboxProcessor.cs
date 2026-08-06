using DevFlow.BuildingBlocks.Infrastructure.Outbox;
using DevFlow.BuildingBlocks.Messaging.Logging;
using DevFlow.BuildingBlocks.Messaging.Serialization;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DevFlow.BuildingBlocks.Messaging.Outbox;

/// <summary>
/// Background service that continuously processes pending
/// integration events stored in the transactional outbox.
/// </summary>
public abstract class OutboxProcessor<TContext> : BackgroundService
    where TContext : class
{
    private const int BatchSize = 20;

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxProcessor<TContext>> _logger;
    private readonly Serialization.IMessageSerializer _serializer;
    private readonly IIntegrationEventTypeResolver _eventTypeResolver;
    private readonly TimeSpan _interval;

    protected OutboxProcessor(
        IServiceScopeFactory scopeFactory,
        ILogger<OutboxProcessor<TContext>> logger,
        Serialization.IMessageSerializer serializer,
        IIntegrationEventTypeResolver eventTypeResolver,
        TimeSpan? interval = null)
    {
        ArgumentNullException.ThrowIfNull(scopeFactory);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(serializer);
        ArgumentNullException.ThrowIfNull(eventTypeResolver);

        _scopeFactory = scopeFactory;
        _logger = logger;
        _serializer = serializer;
        _eventTypeResolver = eventTypeResolver;
        _interval = interval ?? TimeSpan.FromSeconds(10);
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        Console.WriteLine("=== Outbox ExecuteAsync started ===");

        _logger.LogOutboxProcessorStarted(
            _interval.TotalSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingMessagesAsync(
                    stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception exception)
            {
                _logger.LogUnhandledProcessingError(
                    exception);
            }

            try
            {
                await Task.Delay(
                    _interval,
                    stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }

    private async Task ProcessPendingMessagesAsync(
        CancellationToken cancellationToken)
    {
        using IServiceScope scope =
            _scopeFactory.CreateScope();

        var outboxRepository =
            scope.ServiceProvider
                .GetRequiredService<IOutboxRepository>();

        var publishEndpoint =
            scope.ServiceProvider
                .GetRequiredService<IPublishEndpoint>();

        Console.WriteLine("=== Processing Outbox ===");

        var messages =
            await outboxRepository.GetPendingMessagesAsync(
                BatchSize,
                cancellationToken);

        Console.WriteLine($"Pending messages: {messages.Count}");

        if (messages.Count == 0)
        {
            return;
        }

        _logger.LogProcessingMessages(
            messages.Count);

        foreach (var message in messages)
        {
            Console.WriteLine($"Processing: {message.Type}");

            try
            {
                Console.WriteLine("Resolving type...");
                var messageType = _eventTypeResolver.Resolve(message.Type);

                Console.WriteLine($"Resolved: {messageType.FullName}");

                Console.WriteLine("Deserializing...");
                var payload = _serializer.Deserialize(
                    message.Content,
                    messageType);

                if (payload is null)
                {
                    Console.WriteLine("Payload is NULL!");
                    throw new InvalidOperationException("Deserialized payload is null.");
                }

                Console.WriteLine($"Payload type: {payload.GetType().FullName}");

                Console.WriteLine("Publishing...");
                await publishEndpoint.Publish(
                    payload,
                    messageType,
                    cancellationToken);

                Console.WriteLine("Published successfully.");

                message.MarkAsProcessed(DateTime.UtcNow);

                Console.WriteLine("Marked processed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAILED");
                Console.WriteLine(ex);

                message.MarkAsFailed(ex.ToString());
            }
        }

        Console.WriteLine("Saving...");
        await SaveChangesAsync(
            scope.ServiceProvider,
            cancellationToken);

        Console.WriteLine("Saved.");
    }

    /// <summary>
    /// Persists outbox state changes using the service-specific DbContext.
    /// </summary>
    protected abstract Task SaveChangesAsync(
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken);
}
