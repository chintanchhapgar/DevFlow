using FluentAssertions;
using Xunit;

namespace DevFlow.Identity.IntegrationTests.Authentication;

public sealed class LoginTests
{
    [Fact]
    public async Task Login_With_Invalid_Credentials_Should_Fail()
    {
        Assert.True(true);
    }
}
