using DevFlow.Identity.IntegrationTests.Common;
using FluentAssertions;
using Xunit;

namespace DevFlow.Identity.IntegrationTests.Authentication;

public sealed class SmokeTests
    : IntegrationTestBase
{
    public SmokeTests(
        IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task ApiStartsSuccessfully()
    {
        var response =
            await Client.GetAsync("/");

        response.Should().NotBeNull();
    }
}
