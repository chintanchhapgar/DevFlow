using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DevFlow.BuildingBlocks.Observability.HealthChecks;

/// <summary>
/// Configures health checks for service infrastructure dependencies.
/// Exposes /health/live and /health/ready endpoints.
/// </summary>
public static class HealthCheckExtensions
{
    public static IHealthChecksBuilder AddServiceHealthChecks(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);
    }

    public static IHealthChecksBuilder AddPostgresHealthCheck(
        this IHealthChecksBuilder builder,
        IConfiguration configuration,
        string connectionStringKey = "Database:ConnectionString")
    {
        var connectionString = configuration[connectionStringKey];

        if (!string.IsNullOrEmpty(connectionString))
        {
            builder.AddNpgSql(
                connectionString,
                name: "postgresql",
                tags: ["ready", "db"]);
        }

        return builder;
    }

    public static IHealthChecksBuilder AddRedisHealthCheck(
        this IHealthChecksBuilder builder,
        IConfiguration configuration,
        string connectionStringKey = "Redis:ConnectionString")
    {
        var connectionString = configuration[connectionStringKey];

        if (!string.IsNullOrEmpty(connectionString))
        {
            builder.AddRedis(
                connectionString,
                name: "redis",
                tags: ["ready", "cache"]);
        }

        return builder;
    }

    public static IHealthChecksBuilder AddRabbitMqHealthCheck(
        this IHealthChecksBuilder builder,
        IConfiguration configuration,
        string connectionStringKey = "RabbitMq:ConnectionString")
    {
        var connectionString = configuration[connectionStringKey];

        if (!string.IsNullOrEmpty(connectionString))
        {
            builder.AddRabbitMQ(
                rabbitConnectionString: connectionString,
                name: "rabbitmq",
                tags: ["ready", "messaging"]);
        }

        return builder;
    }

    /// <summary>
    /// Maps health check endpoints for Kubernetes liveness and readiness probes.
    /// /health/live — liveness probe (is the process running?)
    /// /health/ready — readiness probe (is the service ready to receive traffic?)
    /// </summary>
    public static IEndpointRouteBuilder MapHealthCheckEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        // Liveness: only checks self
        endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("live"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        // Readiness: checks all dependencies
        endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready") || check.Tags.Contains("live"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        // All checks
        endpoints.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return endpoints;
    }
}
