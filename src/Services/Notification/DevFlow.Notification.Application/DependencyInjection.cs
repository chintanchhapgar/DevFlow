using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.SharedKernel.Domain.DomainEvents;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DevFlow.Notification.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
         this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        Assembly assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(assembly);

        services.AddEndpoints(assembly);

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes =>
                classes.AssignableTo(typeof(IDomainEventConsumer<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
