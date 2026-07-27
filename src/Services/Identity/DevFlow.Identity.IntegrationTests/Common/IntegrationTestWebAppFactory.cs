using DevFlow.Identity.Api;
using DevFlow.Identity.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace DevFlow.Identity.IntegrationTests.Common;

public sealed class IntegrationTestWebAppFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly DatabaseFixture _database = new();

    public async Task InitializeAsync()
    {
        await _database.InitializeAsync();
    }

    public new async Task DisposeAsync()
    {
        await _database.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<IdentityDbContext>>();

            services.AddScoped<TestUserSeeder>();

            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseNpgsql(_database.Container.GetConnectionString());
            });

            var provider = services.BuildServiceProvider();

            using var scope = provider.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

            db.Database.Migrate();

            TestDataSeeder.Seed(db);
        });
    }

    
}
