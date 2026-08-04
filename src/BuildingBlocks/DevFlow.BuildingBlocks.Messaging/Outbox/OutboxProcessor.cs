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

        var messages =
            await outboxRepository.GetPendingMessagesAsync(
                BatchSize,
                cancellationToken);

        if (messages.Count == 0)
        {
            return;
        }

        _logger.LogProcessingMessages(
            messages.Count);

        foreach (var message in messages)
        {
            try
            {
                var messageType =
                    _eventTypeResolver.Resolve(
                        message.Type);

                var payload =
                    _serializer.Deserialize(
                        message.Content,
                        messageType);

                await publishEndpoint.Publish(
                    payload,
                    messageType,
                    cancellationToken);

                var processedOnUtc =
                    DateTime.UtcNow;

                message.MarkAsProcessed(
                    processedOnUtc);

                _logger.LogMessagePublished(
                    message.Id,
                    message.Type);
            }
            catch (Exception exception)
            {
                message.MarkAsFailed(
                    exception.Message);

                _logger.LogMessageProcessingFailed(
                    exception,
                    message.Id);
            }
        }

        await SaveChangesAsync(
            scope.ServiceProvider,
            cancellationToken);
    }

    /// <summary>
    /// Persists outbox state changes using the service-specific DbContext.
    /// </summary>
    protected abstract Task SaveChangesAsync(
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken);
}
