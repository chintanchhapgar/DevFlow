using DevFlow.Identity.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DevFlow.Identity.IntegrationTests.Common;

public abstract class IntegrationTestBase
    : IClassFixture<IntegrationTestWebAppFactory>
{
    protected HttpClient Client { get; }

    protected IServiceScope Scope { get; }

    protected TestUserSeeder Users { get; }

    protected IdentityDbContext Db { get; }

    protected IntegrationTestBase(
        IntegrationTestWebAppFactory factory)
    {
        Client = factory.CreateClient();

        Scope = factory.Services.CreateScope();

        Db = Scope.ServiceProvider
            .GetRequiredService<IdentityDbContext>();

        Users = Scope.ServiceProvider
            .GetRequiredService<TestUserSeeder>();
    }
}
