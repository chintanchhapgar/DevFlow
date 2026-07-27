using System.Security.Claims;

namespace DevFlow.BuildingBlocks.Security.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(
        this ClaimsPrincipal principal)
    {
        var value =
            principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? principal.FindFirstValue("sub");

        return Guid.TryParse(value, out var id)
            ? id
            : Guid.Empty;
    }

    public static string GetEmail(
        this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(
            ClaimTypes.Email) ?? string.Empty;
    }

    public static string GetName(
        this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(
            ClaimTypes.Name) ?? string.Empty;
    }

    public static string GetRole(
        this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(
            ClaimTypes.Role) ?? string.Empty;
    }

    public static Guid GetSessionId(
        this ClaimsPrincipal principal)
    {
        var value =
            principal.FindFirstValue("sid");

        return Guid.TryParse(value, out var id)
            ? id
            : Guid.Empty;
    }
}
