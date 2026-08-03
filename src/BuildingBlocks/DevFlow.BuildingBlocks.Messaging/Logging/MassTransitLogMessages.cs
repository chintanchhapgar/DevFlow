using Microsoft.Extensions.Logging;

namespace DevFlow.BuildingBlocks.Messaging.Logging;

public static class MassTransitLoggingExtensions
{
    private static readonly Action<ILogger, string, Exception?> s_publishing =
        LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(4000, nameof(LogPublishingMessage)),
            "Publishing integration event '{IntegrationEventType}'.");

    private static readonly Action<ILogger, string, Exception?> s_published =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(4001, nameof(LogPublishedMessage)),
            "Successfully published integration event '{IntegrationEventType}'.");

    private static readonly Action<ILogger, string, Exception?> s_publishFailed =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(4002, nameof(LogPublishFailed)),
            "Failed to publish integration event '{IntegrationEventType}'.");

    public static void LogPublishingMessage(
        this ILogger logger,
        string eventType)
    {
        s_publishing(
            logger,
            eventType,
            null);
    }

    public static void LogPublishedMessage(
        this ILogger logger,
        string eventType)
    {
        s_published(
            logger,
            eventType,
            null);
    }

    public static void LogPublishFailed(
        this ILogger logger,
        Exception exception,
        string eventType)
    {
        s_publishFailed(
            logger,
            eventType,
            exception);
    }
}
