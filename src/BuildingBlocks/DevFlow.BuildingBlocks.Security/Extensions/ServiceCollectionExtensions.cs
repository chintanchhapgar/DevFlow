using DevFlow.BuildingBlocks.Security.Authentication;
using DevFlow.BuildingBlocks.Security.Authorization;
using DevFlow.BuildingBlocks.Security.Swagger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevFlow.BuildingBlocks.Security.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSecurity(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddJwtAuthentication(configuration);

        services.AddAuthorizationPolicies();

        services.AddSwaggerGenWithJwt();

        return services;
    }
}
