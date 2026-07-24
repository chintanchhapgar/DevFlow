using DevFlow.BuildingBlocks.Api.Logging;
using DevFlow.BuildingBlocks.Api.Responses;
using DevFlow.SharedKernel.Exceptions;
using DevFlow.SharedKernel.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace DevFlow.BuildingBlocks.Api.Middleware;

/// <summary>
/// Global exception handling middleware.
/// Returns standardized API responses for all unhandled exceptions.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogUnhandledException(
                exception,
                context.Request.Method,
                context.Request.Path);

            await HandleExceptionAsync(
                context,
                exception,
                _environment);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception,
        IHostEnvironment environment)
    {
        var (statusCode, message, code, type) = exception switch
        {
            ValidationException validation => (
                StatusCodes.Status400BadRequest,
                validation.Message,
                "Validation.Error",
                ErrorType.Validation),

            NotFoundException notFound => (
                StatusCodes.Status404NotFound,
                notFound.Message,
                "Resource.NotFound",
                ErrorType.NotFound),

            DomainException domain => (
                StatusCodes.Status400BadRequest,
                domain.Message,
                "Domain.Error",
                ErrorType.Failure),

            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "Authentication is required.",
                "Authentication.Unauthorized",
                ErrorType.Unauthorized),

            _ => (
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred.",
                "Internal.ServerError",
                ErrorType.Unexpected)
        };

        if (environment.IsDevelopment() &&
            exception is not DomainException &&
            exception is not NotFoundException)
        {
            message = exception.Message;
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(
            new ApiResponse<object?>
            {
                Success = false,
                Message = message,
                Data = null,
                Error = new ApiError
                {
                    Code = code,
                    Type = type
                },
                TraceId = context.TraceIdentifier,
                Timestamp = DateTime.UtcNow
            });
    }
}
