using Microsoft.Extensions.Logging;

namespace DevFlow.BuildingBlocks.Messaging.Logging;

internal static partial class OutboxLogMessages
{
    [LoggerMessage(
        EventId = 6000,
        Level = LogLevel.Information,
        Message = "OutboxProcessor started. Interval: {IntervalSeconds}s")]
    public static partial void LogOutboxProcessorStarted(
        this ILogger logger,
        double intervalSeconds);


    [LoggerMessage(
        EventId = 6001,
        Level = LogLevel.Information,
        Message = "Processing {Count} outbox messages")]
    public static partial void LogProcessingMessages(
        this ILogger logger,
        int count);


    [LoggerMessage(
        EventId = 6002,
        Level = LogLevel.Warning,
        Message = "Could not resolve type {Type} for outbox message {Id}")]
    public static partial void LogMessageTypeNotResolved(
        this ILogger logger,
        string type,
        Guid id);


    [LoggerMessage(
        EventId = 6003,
        Level = LogLevel.Debug,
        Message = "Outbox message {Id} of type {Type} published successfully")]
    public static partial void LogMessagePublished(
        this ILogger logger,
        Guid id,
        string type);


    [LoggerMessage(
        EventId = 6004,
        Level = LogLevel.Error,
        Message = "Failed to process outbox message {Id}")]
    public static partial void LogMessageProcessingFailed(
        this ILogger logger,
        Exception exception,
        Guid id);
}
