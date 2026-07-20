using DevFlow.BuildingBlocks.Messaging.EventBus;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DevFlow.BuildingBlocks.Messaging.Configuration;

/// <summary>
/// Extension methods for configuring MassTransit with RabbitMQ.
/// </summary>
public static class MessagingExtensions
{
    /// <summary>
    /// Adds MassTransit with RabbitMQ transport and scans the provided assemblies for consumers.
    /// </summary>
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? configureConsumers = null,
        params Assembly[] consumerAssemblies)
    {
        var settings = configuration
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
                    h =>
                    {
                        h.Username(settings.Username);
                        h.Password(settings.Password);
                    });


                rabbitMq.MessageTopology.SetEntityNameFormatter(
                    new KebabCaseEntityNameFormatter());


                rabbitMq.ConfigureEndpoints(context);
            });
        });


        services.AddScoped<IEventBus, MassTransitEventBus>();

        return services;
    }
}
