using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DevFlow.Project.Application.Common.Abstractions.Identity;

namespace DevFlow.Project.Infrastructure.Identity;

internal sealed class UserLookupService : IUserLookupService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);

    public UserLookupService(HttpClient httpClient)
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

    public async Task<string?> GetNameAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var names = await GetNamesAsync(
            [userId],
            cancellationToken);

        return names.TryGetValue(
            userId,
            out var name)
            ? name
            : null;
    }

    public async Task<IReadOnlyDictionary<Guid, string>> GetNamesAsync(
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default)
    {
        var ids = userIds
            .Where(x => x != Guid.Empty)
            .Distinct()
            .ToArray();

        if (ids.Length == 0)
        {
            return new Dictionary<Guid, string>();
        }

        var query = string.Join(
            "&",
            ids.Select(id =>
                $"Ids={Uri.EscapeDataString(id.ToString())}"));

        var response =
            await _httpClient.GetFromJsonAsync<
                UserNamesResponse>(
                    $"api/users/names?{query}",
                    JsonOptions,
                    cancellationToken);

        if (response?.Value is null)
        {
            return new Dictionary<Guid, string>();
        }

        return response.Value
            .Where(user => ids.Contains(user.Id))
            .ToDictionary(
                user => user.Id,
                user => user.FullName);
    }

    private sealed record UserNamesResponse(
        IReadOnlyList<UserNameResponse>? Value,
        bool IsSuccess,
        bool IsFailure,
        UserErrorResponse? Error,
        string? Message);

    private sealed record UserNameResponse(
        Guid Id,
        string FullName);

    private sealed record UserErrorResponse(
        string Code,
        string Description,
        int Type);
}
