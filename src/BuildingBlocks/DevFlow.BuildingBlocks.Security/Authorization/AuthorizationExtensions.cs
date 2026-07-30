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
                    policy.RequireRole("Admin", "Member");
                });

            options.AddPolicy(
                PolicyNames.ProjectOwner,
                policy =>
                {
                    policy.RequireAuthenticatedUser();
                });

            options.AddPolicy(
                PolicyNames.ProjectEditor,
                policy =>
                {
                    policy.RequireAuthenticatedUser();
                });

            options.AddPolicy(
                PolicyNames.ProjectMember,
                policy =>
                {
                    policy.RequireAuthenticatedUser();
                });

            options.AddPolicy(
                PolicyNames.ProjectViewer,
                policy =>
                {
                    policy.RequireAuthenticatedUser();
                });
        });

        return services;
    }
}
