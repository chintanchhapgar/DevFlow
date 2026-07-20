using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using DevFlow.BuildingBlocks.Api.Behaviors;
using System.Reflection;

namespace DevFlow.Identity.Application.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityApplication(
        this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
        });

        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        return services;
    }
}
