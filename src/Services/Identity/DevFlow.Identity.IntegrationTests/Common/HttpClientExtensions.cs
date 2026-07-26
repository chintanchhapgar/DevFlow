using System.Net.Http.Headers;

namespace DevFlow.Identity.IntegrationTests.Common;

public static class HttpClientExtensions
{
    public static void SetBearerToken(
        this HttpClient client,
        string token)
    {
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);
    }
}
