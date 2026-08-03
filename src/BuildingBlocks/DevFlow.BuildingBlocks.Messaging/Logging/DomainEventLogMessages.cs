using Microsoft.Extensions.Logging;

namespace DevFlow.BuildingBlocks.Messaging.Logging;

public static class DomainEventLogMessages
{
    private static readonly Action<ILogger, string, Exception?> s_dispatchingDomainEvent =
        LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(2000, nameof(LogDispatchingDomainEvent)),
            "Dispatching domain event '{DomainEventType}'.");

    private static readonly Action<ILogger, string, Exception?> s_dispatchedDomainEvent =
        LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(2001, nameof(LogDispatchedDomainEvent)),
            "Successfully dispatched domain event '{DomainEventType}'.");

    private static readonly Action<ILogger, string, Exception?> s_domainEventFailed =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(2002, nameof(LogDomainEventFailed)),
            "Failed while dispatching domain event '{DomainEventType}'.");

    public static void LogDispatchingDomainEvent(
        this ILogger logger,
        string eventType)
    {
        s_dispatchingDomainEvent(
            logger,
            eventType,
            null);
    }

    public static void LogDispatchedDomainEvent(
        this ILogger logger,
        string eventType)
    {
        s_dispatchedDomainEvent(
            logger,
            eventType,
            null);
    }

    public static void LogDomainEventFailed(
        this ILogger logger,
        Exception exception,
        string eventType)
    {
        s_domainEventFailed(
            logger,
            eventType,
            exception);
    }
}
