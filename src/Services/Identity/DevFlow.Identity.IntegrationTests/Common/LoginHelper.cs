using DevFlow.BuildingBlocks.Api.Responses;
using DevFlow.Identity.Application.Authentication.Common;
using DevFlow.Identity.Application.Authentication.Login;
using System.Net.Http.Json;
using Xunit;

namespace DevFlow.Identity.IntegrationTests.Common;

public static class LoginHelper
{
    public static async Task<AuthenticationResponse> LoginAsync(
        HttpClient client,
        string email,
        string password)
    {
        var response = await client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginCommand(email, password));

        response.EnsureSuccessStatusCode();

        var api =
            await response.Content.ReadFromJsonAsync<
                ApiResponse<AuthenticationResponse>>();

        Assert.NotNull(api);
        Assert.NotNull(api.Data);

        return api.Data;
    }
}
