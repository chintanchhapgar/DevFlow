using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Identity;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Infrastructure.Identity;
using DevFlow.Project.Infrastructure.Persistence;
using DevFlow.Project.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevFlow.Project.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ProjectDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("ProjectDb"));
        });

        services.AddScoped<IProjectRepository, ProjectRepository>();

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<ProjectDbContext>());

        services.AddHttpClient<IUserLookupService, UserLookupService>(client =>
        {
            client.BaseAddress = new Uri(
                configuration["Services:Identity"]
                ?? throw new InvalidOperationException(
                    "Identity Service URL is missing."));
        });

        return services;
    }
}
