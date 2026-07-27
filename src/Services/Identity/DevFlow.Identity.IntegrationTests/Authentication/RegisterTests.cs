using System.Net;
using System.Net.Http.Json;
using DevFlow.Identity.Application.Authentication.Register;
using DevFlow.Identity.IntegrationTests.Common;
using Xunit;

namespace DevFlow.Identity.IntegrationTests.Authentication;

public sealed class RegisterTests
    : IntegrationTestBase
{
    public RegisterTests(
        IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_Register_New_User()
    {
        // Arrange

        var command = new RegisterCommand(
            "john@test.com",
            "Password@123",
            "John",
            "Doe");

        // Act

        var response = await Client.PostAsJsonAsync(
            "/api/auth/register",
            command);

        // Assert

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }
}
