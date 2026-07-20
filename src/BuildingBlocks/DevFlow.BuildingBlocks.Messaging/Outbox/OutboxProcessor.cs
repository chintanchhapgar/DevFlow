using DevFlow.BuildingBlocks.Infrastructure.Outbox;
using DevFlow.BuildingBlocks.Messaging.Logging;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DevFlow.BuildingBlocks.Messaging.Outbox;

/// <summary>
/// Background service that processes pending outbox messages.
/// Publishes integration events stored in the outbox.
/// </summary>
public abstract class OutboxProcessor<TContext> : BackgroundService
    where TContext : class
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger _logger;
    private readonly TimeSpan _interval;


    protected OutboxProcessor(
        IServiceScopeFactory scopeFactory,
        ILogger logger,
        TimeSpan? interval = null)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _interval = interval ?? TimeSpan.FromSeconds(10);
    }


    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        _logger.LogOutboxProcessorStarted(
            _interval.TotalSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessPendingMessagesAsync(stoppingToken);

            await Task.Delay(
                _interval,
                stoppingToken);
        }
    }


    private async Task ProcessPendingMessagesAsync(
        CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var outboxRepository = scope.ServiceProvider
            .GetRequiredService<IOutboxRepository>();

        var publishEndpoint = scope.ServiceProvider
            .GetRequiredService<IPublishEndpoint>();


        var messages = await outboxRepository
            .GetPendingMessagesAsync(
                20,
                cancellationToken);


        if (messages.Count == 0)
            return;


        _logger.LogProcessingMessages(
            messages.Count);


        foreach (var message in messages)
        {
            try
            {
                var messageType = Type.GetType(message.Type);

                if (messageType is null)
                {
                    _logger.LogMessageTypeNotResolved(
                        message.Type,
                        message.Id);

                    continue;
                }


                var payload = JsonSerializer.Deserialize(
                    message.Content,
                    messageType);


                if (payload is null)
                    continue;


                await publishEndpoint.Publish(
                    payload,
                    messageType,
                    cancellationToken);


                message.MarkAsProcessed(
                    DateTime.UtcNow);


                _logger.LogMessagePublished(
                    message.Id,
                    message.Type);
            }
            catch (Exception exception)
            {
                _logger.LogMessageProcessingFailed(
                    exception,
                    message.Id);

                message.MarkAsFailed(
                    exception.Message);
            }
        }


        await SaveChangesAsync(
            scope.ServiceProvider,
            cancellationToken);
    }


    /// <summary>
    /// Saves outbox changes using the service-specific DbContext.
    /// </summary>
    protected abstract Task SaveChangesAsync(
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken);
}
