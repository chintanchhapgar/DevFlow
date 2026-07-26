using System.Net.Http.Json;
using Xunit;

namespace DevFlow.Identity.IntegrationTests.Common;

public abstract class IntegrationTestBase
    : IClassFixture<IntegrationTestWebAppFactory>
{
    protected HttpClient Client { get; }

    protected IntegrationTestWebAppFactory Factory { get; }

    protected IntegrationTestBase(
        IntegrationTestWebAppFactory factory)
    {
        Factory = factory;

        Client = factory.CreateClient();
    }
}
