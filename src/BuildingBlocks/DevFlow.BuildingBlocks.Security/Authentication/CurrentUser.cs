using DevFlow.SharedKernel.Common;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DevFlow.BuildingBlocks.Security.Authentication;

internal sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User =>
        _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated ?? false;

    public Guid UserId =>
        User?.GetUserId() ?? Guid.Empty;

    public string Email =>
        User?.GetEmail() ?? string.Empty;

    public string Name =>
        User?.GetName() ?? string.Empty;

    public string Role =>
        User?.GetRole() ?? string.Empty;

    public Guid SessionId =>
        User?.GetSessionId() ?? Guid.Empty;
}
