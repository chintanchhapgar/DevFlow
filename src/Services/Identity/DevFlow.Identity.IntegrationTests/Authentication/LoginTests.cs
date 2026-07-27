using DevFlow.BuildingBlocks.Api.Responses;
using DevFlow.Identity.Application.Authentication.Common;
using DevFlow.Identity.Application.Authentication.Login;
using DevFlow.Identity.IntegrationTests.Common;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace DevFlow.Identity.IntegrationTests.Authentication;

public sealed class LoginTests : IntegrationTestBase
{
    public LoginTests(
        IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_Login_With_Valid_Credentials()
    {
        const string password = "Password@123";

        var testUser = await Users.CreateAsync(
            password: password);

        var response = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginCommand(
                testUser.Email,
                password));

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var api =
            await response.Content.ReadFromJsonAsync<
                ApiResponse<AuthenticationResponse>>();

        Assert.NotNull(api);
        Assert.True(api.Success);
        Assert.NotNull(api.Data);

        Assert.False(api.Data.RequiresTwoFactor);

        Assert.False(
            string.IsNullOrWhiteSpace(
                api.Data.AccessToken));

        Assert.False(
            string.IsNullOrWhiteSpace(
                api.Data.RefreshToken));
    }

    [Fact]
    public async Task Should_Reject_Invalid_Password()
    {
        var testUser = await Users.CreateAsync();

        var response = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginCommand(
                testUser.Email,
                "WrongPassword"));

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Should_Reject_Unknown_Email()
    {
        var response = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginCommand(
                "unknown@test.com",
                "Password@123"));

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Should_Reject_Disabled_User()
    {
        const string password = "Password@123";

        var testUser = await Users.CreateAsync(
            password: password);

        var user = await Users.GetUserAsync(
            testUser.Id);

        user.Deactivate();

        await Db.SaveChangesAsync();

        var response = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginCommand(
                testUser.Email,
                password));

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Should_Login_When_TwoFactor_Is_Enabled()
    {
        const string password = "Password@123";

        var testUser = await Users.CreateAsync(
            password: password);

        var user = await Users.GetUserAsync(
            testUser.Id);

        user.BeginTwoFactorSetup(
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ");

        user.CompleteTwoFactorSetup();

        await Db.SaveChangesAsync();

        var response = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginCommand(
                testUser.Email,
                password));

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var api =
            await response.Content.ReadFromJsonAsync<
                ApiResponse<AuthenticationResponse>>();

        Assert.NotNull(api);
        Assert.True(api.Success);
        Assert.NotNull(api.Data);

        Assert.True(
            api.Data.RequiresTwoFactor);

        Assert.NotNull(
            api.Data.UserId);

        Assert.True(
            string.IsNullOrWhiteSpace(
                api.Data.AccessToken));

        Assert.True(
            string.IsNullOrWhiteSpace(
                api.Data.RefreshToken));
    }

    [Fact]
    public async Task Should_Reject_Locked_User()
    {
        const string password = "Password@123";

        var testUser = await Users.CreateAsync(
            password: password);

        var user = await Users.GetUserAsync(
            testUser.Id);

        for (var i = 0; i < 5; i++)
        {
            user.RecordFailedLogin(
                5,
                TimeSpan.FromMinutes(15));
        }

        await Db.SaveChangesAsync();

        var response = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginCommand(
                testUser.Email,
                password));

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Should_Reset_Failed_Login_Count_After_Successful_Login()
    {
        const string password = "Password@123";

        var testUser = await Users.CreateAsync(
            password: password);

        var user = await Users.GetUserAsync(
            testUser.Id);

        user.RecordFailedLogin(
            5,
            TimeSpan.FromMinutes(15));

        user.RecordFailedLogin(
            5,
            TimeSpan.FromMinutes(15));

        await Db.SaveChangesAsync();

        var response = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginCommand(
                testUser.Email,
                password));

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        Db.ChangeTracker.Clear();

        var dbUser =
            await Users.GetUserAsync(
                testUser.Id);

        Assert.Equal(
            0,
            dbUser.AccessFailedCount);

        Assert.Null(
            dbUser.LockoutEndUtc);
    }

    [Fact]
    public async Task Should_Lock_User_After_Maximum_Failed_Attempts()
    {
        const string password = "Password@123";

        var testUser = await Users.CreateAsync(
            password: password);

        for (var i = 0; i < 5; i++)
        {
            await Client.PostAsJsonAsync(
                "/api/auth/login",
                new LoginCommand(
                    testUser.Email,
                    "WrongPassword"));
        }

        Db.ChangeTracker.Clear();

        var user =
            await Users.GetUserAsync(
                testUser.Id);

        Assert.True(
            user.IsLockedOut);
    }
}
