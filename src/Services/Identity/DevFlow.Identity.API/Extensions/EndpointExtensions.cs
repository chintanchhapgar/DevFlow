using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.Identity.Application.Authentication.Login;
using Microsoft.AspNetCore.Routing;
using System.Reflection;

namespace DevFlow.Identity.Api.Extensions;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(
        this IEndpointRouteBuilder app)
    {
        var assemblies = new[]
        {
            typeof(Program).Assembly,      // Identity.Api
            typeof(LoginCommand).Assembly  // Identity.Application
        };

        var endpoints = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                typeof(IEndpoint).IsAssignableFrom(t) &&
                !t.IsAbstract &&
                !t.IsInterface);

        foreach (var type in endpoints)
        {
            ((IEndpoint)Activator.CreateInstance(type)!)
                .MapEndpoint(app);
        }

        return app;
    }
}
