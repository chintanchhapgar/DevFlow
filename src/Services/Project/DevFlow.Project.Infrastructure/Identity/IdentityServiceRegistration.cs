
using DevFlow.Project.Application.Common.Abstractions.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DevFlow.Project.Infrastructure.Identity;

public static class IdentityServiceRegistration
{
    public static IServiceCollection AddIdentityIntegration(
        this IServiceCollection services,
        string identityBaseUrl)
    {
        services.AddHttpContextAccessor();

        services.AddTransient<
            ForwardAuthorizationHandler>();

        services.AddHttpClient<
            IUserLookupService,
            UserLookupService>(client =>
            {
                client.BaseAddress =
                    new Uri(identityBaseUrl.TrimEnd('/') + "/");

                client.Timeout =
                    TimeSpan.FromSeconds(10);
            })
            .AddHttpMessageHandler<
                ForwardAuthorizationHandler>();

        return services;
    }
}
