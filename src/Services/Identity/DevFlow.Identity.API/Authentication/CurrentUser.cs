using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Domain.Authentication.Users;
using System.Security.Claims;

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

    public UserId UserId =>
        new UserId(Guid.Parse(
            _httpContextAccessor.HttpContext!
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier)!));

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
