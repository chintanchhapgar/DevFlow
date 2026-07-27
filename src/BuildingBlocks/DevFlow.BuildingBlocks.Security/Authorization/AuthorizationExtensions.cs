using Microsoft.Extensions.DependencyInjection;

namespace DevFlow.BuildingBlocks.Security.Authorization;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationPolicies(
        this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                PolicyNames.Admin,
                policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Admin");
                });

            options.AddPolicy(
                PolicyNames.Member,
                policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(
                        "Admin",
                        "Member");
                });

            // Placeholder.
            // Will be implemented after Project Permissions service.
            options.AddPolicy(
                PolicyNames.ProjectOwner,
                policy =>
                {
                    policy.RequireAuthenticatedUser();
                });

            // Placeholder.
            // Will be implemented after Project Permissions service.
            options.AddPolicy(
                PolicyNames.ProjectMember,
                policy =>
                {
                    policy.RequireAuthenticatedUser();
                });
        });

        return services;
    }
}
