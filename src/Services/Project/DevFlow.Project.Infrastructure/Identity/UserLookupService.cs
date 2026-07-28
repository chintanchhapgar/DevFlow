using System.Net;
using DevFlow.Project.Application.Common.Abstractions.Identity;

namespace DevFlow.Project.Infrastructure.Identity;

internal sealed class UserLookupService
    : IUserLookupService
{
    private readonly HttpClient _httpClient;

    public UserLookupService(
        HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ExistsAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            $"api/users/{userId}/exists",
            cancellationToken);

        return response.StatusCode == HttpStatusCode.OK;
    }
}
