using System.Text.Json;
using DevFlow.BuildingBlocks.Api.Logging;
using DevFlow.BuildingBlocks.Api.ProblemDetails;
using DevFlow.SharedKernel.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DevFlow.BuildingBlocks.Api.Middleware;

/// <summary>
/// Global exception handler middleware.
/// Converts unhandled exceptions into RFC 7807 Problem Details responses.
/// Prevents stack traces from leaking to clients.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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
                exception);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var (statusCode, title, detail) = exception switch
        {
            NotFoundException notFound => (
                StatusCodes.Status404NotFound,
                "Not Found",
                notFound.Message),

            DomainException domain => (
                StatusCodes.Status400BadRequest,
                "Domain Rule Violation",
                domain.Message),

            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "Unauthorized",
                "Authentication is required."),

            _ => (
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                "An unexpected error occurred. Please try again later.")
        };

        var problemDetails = new ProblemDetailsViewModel
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance =
                $"{context.Request.Method} {context.Request.Path}"
        };

        context.Response.ContentType =
            "application/problem+json";

        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(
            problemDetails,
            JsonOptions);
    }
}
