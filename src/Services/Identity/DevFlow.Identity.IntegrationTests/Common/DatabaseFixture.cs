using Testcontainers.PostgreSql;
using Xunit;

namespace DevFlow.Identity.IntegrationTests.Common;

public sealed class DatabaseFixture
    : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; } =
        new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithDatabase("identity-tests")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

    public Task InitializeAsync()
        => Container.StartAsync();

    public Task DisposeAsync()
        => Container.DisposeAsync().AsTask();
}
