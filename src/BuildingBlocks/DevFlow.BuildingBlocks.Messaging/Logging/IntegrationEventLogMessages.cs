using Microsoft.Extensions.Logging;

namespace DevFlow.BuildingBlocks.Messaging.Logging;

public static class IntegrationEventLogMessages
{
    private static readonly Action<ILogger, string, Exception?> s_storingIntegrationEvent =
        LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(3000, nameof(LogStoringIntegrationEvent)),
            "Storing integration event '{IntegrationEventType}' in outbox.");

    private static readonly Action<ILogger, string, Exception?> s_integrationEventStored =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(3001, nameof(LogIntegrationEventStored)),
            "Integration event '{IntegrationEventType}' stored successfully.");

    public static void LogStoringIntegrationEvent(
        this ILogger logger,
        string eventType)
    {
        s_storingIntegrationEvent(
            logger,
            eventType,
            null);
    }

    public static void LogIntegrationEventStored(
        this ILogger logger,
        string eventType)
    {
        s_integrationEventStored(
            logger,
            eventType,
            null);
    }
}
