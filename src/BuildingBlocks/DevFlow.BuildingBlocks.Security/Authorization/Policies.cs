using DevFlow.BuildingBlocks.Security.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace DevFlow.BuildingBlocks.Api.Authentication;

public static class Policies
{
    public static IServiceCollection AddAuthorizationPolicies(
        this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                PolicyNames.Admin,
                p => p.RequireRole("Admin"));

            options.AddPolicy(
                PolicyNames.Member,
                p => p.RequireRole("Member"));
        });

        return services;
    }
}
