using DevFlow.Identity.IntegrationTests.Common;
using FluentAssertions;
using Xunit;

namespace DevFlow.Identity.IntegrationTests.Authentication;

public sealed class UserSeederTests
    : IntegrationTestBase
{
    public UserSeederTests(
        IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_Create_Test_User()
    {
        var user = await Users.CreateAsync();

        user.Should().NotBeNull();

        user.Email.Should().NotBeNullOrWhiteSpace();

        user.Password.Should().Be("Password@123");
    }
}
