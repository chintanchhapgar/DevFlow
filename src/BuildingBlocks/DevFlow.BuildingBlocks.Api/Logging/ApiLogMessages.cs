using Microsoft.Extensions.Logging;

namespace DevFlow.BuildingBlocks.Api.Logging;

internal static partial class ApiLogMessages
{
    [LoggerMessage(
        EventId = 5000,
        Level = LogLevel.Error,
        Message = "Unhandled exception for {Method} {Path}")]
    public static partial void LogUnhandledException(
        this ILogger logger,
        Exception exception,
        string method,
        string? path);


    [LoggerMessage(
        EventId = 5001,
        Level = LogLevel.Information,
        Message = "HTTP {Method} {Path} started")]
    public static partial void LogRequestStarted(
        this ILogger logger,
        string method,
        string? path);


    [LoggerMessage(
        EventId = 5002,
        Level = LogLevel.Information,
        Message = "HTTP {Method} {Path} responded {StatusCode} in {Duration}ms")]
    public static partial void LogRequestCompleted(
        this ILogger logger,
        string method,
        string? path,
        int statusCode,
        long duration);


    [LoggerMessage(
        EventId = 5003,
        Level = LogLevel.Error,
        Message = "HTTP {Method} {Path} failed after {Duration}ms")]
    public static partial void LogRequestFailed(
        this ILogger logger,
        Exception exception,
        string method,
        string? path,
        long duration);
}
