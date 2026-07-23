using System.Security.Claims;
using DevFlow.Identity.Application.Common.Abstractions.Authentication;

namespace DevFlow.Identity.API.Authentication;

internal sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User =>
        _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated ?? false;

    public Guid UserId =>
        Guid.TryParse(
            User?.FindFirstValue(ClaimTypes.NameIdentifier),
            out var id)
            ? id
            : Guid.Empty;

    public string Email =>
        User?.FindFirstValue(ClaimTypes.Email)
        ?? string.Empty;

    public string Role =>
        User?.FindFirstValue(ClaimTypes.Role)
        ?? string.Empty;

    public string Name =>
        User?.FindFirstValue(ClaimTypes.Name)
        ?? string.Empty;
}
