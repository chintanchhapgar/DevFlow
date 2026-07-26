using DevFlow.SharedKernel.Common;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DevFlow.Identity.Api.Authentication;

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

    public Guid UserId
    {
        get
        {
            var value =
                User?.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User?.FindFirstValue("sub");

            return Guid.TryParse(value, out var id)
                ? id
                : Guid.Empty;
        }
    }

    public string Email =>
      User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

    public string Name =>
        User?.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

    public string Role =>
        User?.FindFirstValue(ClaimTypes.Role) ?? string.Empty;

    public Guid SessionId
    {
        get
        {
            var value =
                User?.FindFirstValue(JwtRegisteredClaimNames.Jti);

            return Guid.TryParse(value, out var id)
                ? id
                : Guid.Empty;
        }
    }
}
