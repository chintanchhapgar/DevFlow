using DevFlow.Project.Infrastructure.Seed.Projects;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DevFlow.Project.Infrastructure.Seed;

public static class SeedExtensions
{
    public static async Task SeedDemoDataAsync(
        this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var seeder =
            scope.ServiceProvider
                .GetRequiredService<ProjectSeeder>();

        await seeder.SeedAsync();
    }
}
