
using DevFlow.BuildingBlocks.Infrastructure.DomainEvents;
using Microsoft.Extensions.DependencyInjection;

namespace DevFlow.BuildingBlocks.Messaging;

internal static class DispatchingRegistration
{
    internal static IServiceCollection AddDomainEventDispatching(
        this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
}
