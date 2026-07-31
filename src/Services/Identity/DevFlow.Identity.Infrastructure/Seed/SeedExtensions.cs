using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DevFlow.Identity.Infrastructure.Seed;

public static class SeedExtensions
{
    public static async Task SeedDemoDataAsync(
        this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var seeder =
            scope.ServiceProvider
                .GetRequiredService<IdentitySeeder>();

        await seeder.SeedAsync();
    }
}
