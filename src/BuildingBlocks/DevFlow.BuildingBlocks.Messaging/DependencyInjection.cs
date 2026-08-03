using DevFlow.BuildingBlocks.Messaging.Configuration;
using DevFlow.BuildingBlocks.Messaging.EventBus;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DevFlow.BuildingBlocks.Messaging;

/// <summary>
/// Registers the complete messaging infrastructure.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddMessaging<TContext>(
    this IServiceCollection services,
    IConfiguration configuration,
    Action<IBusRegistrationConfigurator>? configureConsumers = null,
    params Assembly[] consumerAssemblies)
    where TContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services
            .AddSerialization()
            .AddDomainEventDispatching()
            .AddIntegrationEvents()
            .AddOutbox<TContext>();

        RegisterMassTransit(
            services,
            configuration,
            configureConsumers,
            consumerAssemblies);

        return services;
    }

    private static void RegisterMassTransit(
        IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? configureConsumers,
        Assembly[] consumerAssemblies)
    {
        var settings =
            configuration
                .GetSection(RabbitMqSettings.SectionName)
                .Get<RabbitMqSettings>()
            ?? new RabbitMqSettings();

        services.AddMassTransit(bus =>
        {
            foreach (var assembly in consumerAssemblies)
            {
                bus.AddConsumers(assembly);
            }

            configureConsumers?.Invoke(bus);

            bus.UsingRabbitMq((context, rabbitMq) =>
            {
                rabbitMq.Host(
                    new Uri(
                        $"rabbitmq://{settings.Host}:{settings.Port}/{settings.VirtualHost}"),
                    host =>
                    {
                        host.Username(settings.Username);
                        host.Password(settings.Password);
                    });

                rabbitMq.MessageTopology.SetEntityNameFormatter(
                    new KebabCaseEntityNameFormatter());

                rabbitMq.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<IEventBus, MassTransitEventBus>();
    }
}
