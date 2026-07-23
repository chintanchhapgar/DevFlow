using DevFlow.SharedKernel.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DevFlow.BuildingBlocks.Infrastructure.Services;

/// <summary>
/// Resolves the current user from the HTTP request's ClaimsPrincipal.
/// </summary>
public sealed class CurrentUserService : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? Principal =>
        _httpContextAccessor.HttpContext?.User;

    public Guid? UserId
    {
        get
        {
            var claim = Principal?.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? Principal?.FindFirstValue("sub");

            return Guid.TryParse(claim, out var userId) ? userId : null;
        }
    }

    public string? Email =>
        Principal?.FindFirstValue(ClaimTypes.Email)
        ?? Principal?.FindFirstValue("email");

    public bool IsAuthenticated =>
        Principal?.Identity?.IsAuthenticated ?? false;
}
