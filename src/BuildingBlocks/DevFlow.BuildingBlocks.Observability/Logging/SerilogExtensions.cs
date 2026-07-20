using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System.Globalization;

namespace DevFlow.BuildingBlocks.Observability.Logging;

/// <summary>
/// Configures Serilog for structured logging across all services.
/// </summary>
public static class SerilogExtensions
{
    public static WebApplicationBuilder AddSerilogLogging(
        this WebApplicationBuilder builder,
        string serviceName)
    {
        builder.Host.UseSerilog((context, services, config) =>
        {
            config
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithThreadId()
                .Enrich.WithProperty("ServiceName", serviceName)
                .Enrich.WithProperty(
                    "Version",
                    System.Reflection.Assembly.GetEntryAssembly()
                        ?.GetName().Version?.ToString() ?? "unknown")
                .WriteTo.Console(
                    outputTemplate:
                        "[{Timestamp:HH:mm:ss} {Level:u3}] [{ServiceName}] {Message:lj}{NewLine}{Exception}",
                    formatProvider: CultureInfo.InvariantCulture)
                .WriteTo.Seq(
                    context.Configuration["Seq:ServerUrl"] ?? "http://localhost:5341",
                    restrictedToMinimumLevel: LogEventLevel.Information);

            if (context.HostingEnvironment.IsDevelopment())
            {
                config.MinimumLevel.Debug();
            }
            else
            {
                config.MinimumLevel.Information();
            }

            config
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .MinimumLevel.Override("MassTransit", LogEventLevel.Warning);
        });

        return builder;
    }
}
