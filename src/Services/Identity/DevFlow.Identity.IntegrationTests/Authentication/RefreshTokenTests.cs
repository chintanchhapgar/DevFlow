using DevFlow.BuildingBlocks.Api.Responses;
using DevFlow.Identity.Application.Authentication.Common;
using DevFlow.Identity.Application.Authentication.Login;
using DevFlow.Identity.Application.Authentication.RefreshToken;
using DevFlow.Identity.IntegrationTests.Common;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace DevFlow.Identity.IntegrationTests.Authentication;

public sealed class RefreshTokenTests
    : IntegrationTestBase
{
    public RefreshTokenTests(
        IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_Refresh_Access_Token()
    {
        //// Arrange

        //const string password = "Password@123";

        //var testUser = await Users.CreateAsync(
        //    password: password);

        //var loginResponse = await Client.PostAsJsonAsync(
        //    "/api/auth/login",
        //    new LoginCommand(
        //        testUser.Email,
        //        password));

        //Assert.Equal(
        //    HttpStatusCode.OK,
        //    loginResponse.StatusCode);

        //var loginApi =
        //    await loginResponse.Content.ReadFromJsonAsync<
        //        ApiResponse<AuthenticationResponse>>();

        //Assert.NotNull(loginApi);
        //Assert.True(loginApi.Success);
        //Assert.NotNull(loginApi.Data);

        //var login = loginApi.Data;

        //// Act

        //var refreshResponse = await Client.PostAsJsonAsync(
        //    "/api/auth/refresh",
        //    new RefreshTokenCommand(
        //        login.RefreshToken!));

        //// Assert

        //var body = await refreshResponse.Content.ReadAsStringAsync();

        //Assert.True(
        //    refreshResponse.IsSuccessStatusCode,
        //    body);

        //var refreshApi =
        //    await refreshResponse.Content.ReadFromJsonAsync<
        //        ApiResponse<RefreshTokenResponse>>();

        //Assert.NotNull(refreshApi);

        //Assert.True(refreshApi.Success);

        //Assert.NotNull(refreshApi.Data);

        //Assert.False(
        //    string.IsNullOrWhiteSpace(
        //        refreshApi.Data.AccessToken));

        //Assert.False(
        //    string.IsNullOrWhiteSpace(
        //        refreshApi.Data.RefreshToken));

        //Assert.NotEqual(
        //    login.AccessToken,
        //    refreshApi.Data.AccessToken);

        //Assert.NotEqual(
        //    login.RefreshToken,
        //    refreshApi.Data.RefreshToken);
    }
}
