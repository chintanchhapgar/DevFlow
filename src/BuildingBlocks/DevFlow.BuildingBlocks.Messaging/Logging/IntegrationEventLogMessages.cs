using Microsoft.Extensions.Logging;

namespace DevFlow.BuildingBlocks.Messaging.Logging;

public static partial class IntegrationEventLogMessages
{
    [LoggerMessage(
        EventId = 7000,
        Level = LogLevel.Information,
        Message = "User registered: {Email}")]
    public static partial void LogUserRegistered(
        this ILogger logger,
        string email);

    [LoggerMessage(
        EventId = 7001,
        Level = LogLevel.Information,
        Message = "Received integration event {EventType}")]
    public static partial void LogIntegrationEventReceived(
        this ILogger logger,
        string eventType);

    [LoggerMessage(
        EventId = 7002,
        Level = LogLevel.Information,
        Message = "Published integration event {EventType}")]
    public static partial void LogIntegrationEventPublished(
        this ILogger logger,
        string eventType);

    [LoggerMessage(
        EventId = 7003,
        Level = LogLevel.Error,
        Message = "Failed processing integration event {EventType}")]
    public static partial void LogIntegrationEventFailed(
        this ILogger logger,
        Exception exception,
        string eventType);

    [LoggerMessage(
    EventId = 7004,
    Level = LogLevel.Information,
    Message = "Project created: {ProjectName}")]
    public static partial void LogProjectCreated(
    this ILogger logger,
    string projectName);
}
