using System.Net.Http.Json;

namespace DevFlow.Identity.IntegrationTests.Common;

public static class JsonExtensions
{
    public static async Task<T?> ReadAsAsync<T>(
        this HttpResponseMessage response)
    {
        return await response.Content.ReadFromJsonAsync<T>();
    }
}
